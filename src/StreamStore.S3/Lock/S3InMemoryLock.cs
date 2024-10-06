
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;

namespace StreamStore.S3.Lock
{
    internal class S3StreamInMemoryLock : IS3StreamLock
    {
        readonly S3StreamLockStorage storage;
        readonly Id streamId;

        public S3StreamInMemoryLock(Id streamId, S3StreamLockStorage storage)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));
            this.streamId = streamId;
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {
            var lockId = storage.TryLock(streamId);
            if (lockId == null) return Task.FromResult<IS3LockHandle?>(null);

            return Task.FromResult<IS3LockHandle?>(S3StreamInMemoryLockHandle.New(storage, streamId));
        }
    }

    class S3StreamLockStorage
    {
        readonly ConcurrentDictionary<Id, Guid> locks = new ConcurrentDictionary<Id, Guid>();

        public Guid? TryLock(Id streamId)
        {
            if (locks.ContainsKey(streamId)) return null;

            var lockId = Guid.NewGuid();
            if (locks.TryAdd(streamId, lockId)) return lockId;
            return null;
        }

        public void ReleaseLock(Id streamId)
        {
            locks.TryRemove(streamId, out _);
        }
    }

    class S3StreamInMemoryLockHandle : IS3LockHandle
    {
        S3StreamLockStorage? storage;
        readonly Id streamId;

        S3StreamInMemoryLockHandle(S3StreamLockStorage storage, Id streamId)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));
            this.streamId = streamId;
        }

        public Task ReleaseAsync(CancellationToken token)
        {
            storage!.ReleaseLock(streamId);
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await ReleaseAsync(CancellationToken.None);
                storage = null;
            }
        }

        public static S3StreamInMemoryLockHandle New(S3StreamLockStorage storage, Id streamId)
        {
            return new S3StreamInMemoryLockHandle(storage, streamId);
        }
    }
}
