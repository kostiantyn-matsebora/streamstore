using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;


namespace StreamStore.S3.Lock
{
    class S3FileLockHandle : IS3LockHandle
    {
        bool lockReleased = false;
        readonly IS3Object lockObject;

        public S3FileLockHandle(IS3Object lockObject)
        {
            this.lockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject));
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            if (!lockReleased)
            {
                lockReleased = true;
                await lockObject.DeleteAsync(token);
            }
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
            }
        }
    }
}
