using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using B2Net;
using B2Net.Models;
using StreamStore.S3.Client;


namespace StreamStore.S3.B2
{
    internal class B2S3StreamLock : IS3StreamLock
    {
        Id streamId;
        string? bucketId;
        string? bucketName;
        B2Client? client;
        string? fileId;
        bool lockAcquired;
        bool inMemoryLockAcquired;
        Guid? lockId;

        static readonly ConcurrentDictionary<Id, Guid> streamLocks = new ConcurrentDictionary<Id, Guid>();

        public B2S3StreamLock(Id streamId, string bucketId, string bucketName, B2Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));

            this.streamId = streamId;
            this.bucketId = bucketId ?? throw new ArgumentNullException(nameof(bucketId));
            this.bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.lockId = Guid.NewGuid();
        }

        public async Task<bool> AcquireAsync(CancellationToken token)
        {
            if (lockAcquired)
                throw new InvalidOperationException("Lock already acquired.");

            // Trying to acquire in-memory lock
            if (!TryAcquireInMemoryLock())
                return false;

            // Trying to figure out if lock already acquired
            var existingLockId = await GetExistingLockId(token);
            if (existingLockId != null) return false;

            if (!(await TryUploadLock(token))) return false;

            // Trying to figure out if lock already acquired
            existingLockId = await GetExistingLockId(token);
            if (lockId != existingLockId) return false;

            lockAcquired = true;
            return true;
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            // Removing in-memory lock
            if (inMemoryLockAcquired)
                ReleaseInMemoryLock();

            // Removing file lock
            if (!string.IsNullOrEmpty(fileId))
                await DeleteFile(token);
        }

        async Task<bool> TryUploadLock(CancellationToken token)
        {
            var uploadUrl = await client!.Files.GetUploadUrl(bucketId);
            var lockData = Encoding.UTF8.GetBytes(lockId.ToString());
            var file = await client!.Files.Upload(lockData, S3Naming.LockKey(streamId), uploadUrl, string.Empty, null, token);
            if (file == null)
                return false;

            fileId = file.FileId;
            return true;
        }

        bool TryAcquireInMemoryLock()
        {
            if (streamLocks.ContainsKey(streamId))
                return false;

            var lockObject = new object();
            var result =  streamLocks.TryAdd(streamId, lockId!.Value);
            inMemoryLockAcquired = result;
            return result;
        }


        private async Task DeleteFile(CancellationToken token)
        {
            await client!.Files.Delete(fileId, S3Naming.LockKey(streamId), token);
            fileId = null;
        }

        private void ReleaseInMemoryLock()
        {
            streamLocks.Remove(streamId, out _);
            inMemoryLockAcquired = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseAsync(CancellationToken.None).Wait();
                streamId = Id.None;
                lockId = null;
                bucketId = null;
                bucketName = null;
                client = null;
                fileId = null;
                lockAcquired = false;
            }
        }

        async Task<Guid?> GetExistingLockId(CancellationToken token)
        {
            try
            {
              var file = await client!.Files.DownloadByName(S3Naming.LockKey(streamId), bucketName, token);
              var existingLockId = new Guid(Encoding.UTF8.GetString(file.FileData));
              return existingLockId;

            } catch (B2Exception ex)
            {
                if (ex.Code == "not_found")
                    return null;
                throw;
            }
        }
    }
}
