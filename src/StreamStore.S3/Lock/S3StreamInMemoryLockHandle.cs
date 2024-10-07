
using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;

namespace StreamStore.S3.Lock
{
    class S3StreamInMemoryLockHandle : IS3LockHandle
    {
        S3InMemoryStreamLockStorage? storage;
        readonly Id streamId;

        S3StreamInMemoryLockHandle(S3InMemoryStreamLockStorage storage, Id streamId)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));
            this.streamId = streamId;
        }

        public Task ReleaseAsync(CancellationToken token)
        {
            storage!.TryRemove(streamId);
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

        public static S3StreamInMemoryLockHandle New(S3InMemoryStreamLockStorage storage, Id streamId)
        {
            return new S3StreamInMemoryLockHandle(storage, streamId);
        }
    }
}
