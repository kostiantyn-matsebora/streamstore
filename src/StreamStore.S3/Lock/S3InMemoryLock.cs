
using System;
using System.Threading;
using System.Threading.Tasks;

using StreamStore.S3.Client;

namespace StreamStore.S3.Lock
{
    internal class S3StreamInMemoryLock : IS3StreamLock
    {
        readonly S3InMemoryStreamLockStorage storage;
        readonly Id streamId;

        public S3StreamInMemoryLock(Id streamId, S3InMemoryStreamLockStorage storage)
        {
         
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));
            this.streamId = streamId;
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {
            var lockId = storage.TryAdd(streamId);
            if (lockId == null) return Task.FromResult<IS3LockHandle?>(null);

            return Task.FromResult<IS3LockHandle?>(S3StreamInMemoryLockHandle.New(storage, streamId));
        }
    }
}
