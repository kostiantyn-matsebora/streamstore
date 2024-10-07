﻿using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Lock
{
    partial class S3FileLock : IS3StreamLock
    {
        readonly Id streamId;
        readonly IS3Factory factory;

        public S3FileLock(Id streamId, IS3Factory factory)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));

            this.streamId = streamId;

            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {

            var lockId = new LockId();

            // Trying to figure out if lock already acquired
            await using var client = factory.CreateClient();
            var response = await client.FindObjectAsync(S3Naming.LockKey(streamId), token);
            if (response != null) return null;

            // Upload lock
            var request = new UploadObjectRequest
            {
                Key = S3Naming.LockKey(streamId),
                Data = Converter.ToByteArray(lockId),
            };

            var uploadResponse = await client.UploadObjectAsync(request, token);
            if (uploadResponse == null) return null;

            // Trying to figure out if lock already acquired
            var result = await client.FindObjectAsync(S3Naming.LockKey(streamId), token);
            if (result == null) return null;

            var existingLockId = Converter.FromByteArray<LockId>(result!.Data!);

            if (!lockId.Equals(existingLockId!)) return null;

            return new S3FileLockHandle(streamId, uploadResponse!.FileId!, factory);
        }
    }
}