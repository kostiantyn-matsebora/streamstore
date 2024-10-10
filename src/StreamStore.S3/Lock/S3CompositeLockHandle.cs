using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;

namespace StreamStore.S3.Lock
{
    class S3CompositeLockHandle : IS3LockHandle
    {
        IS3LockHandle[]? handles;
        bool lockReleased;

        public S3CompositeLockHandle(params IS3LockHandle[] handles)
        {
            this.handles = handles ?? throw new ArgumentNullException(nameof(handles));
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {

                foreach (var handle in handles!)
                {
                    handle.DisposeAsync();
                }

                handles = null;
            }
            return Task.CompletedTask;
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            if (lockReleased) return ;
            lockReleased = true;

            foreach (var handle in handles!)
            {
                await handle.ReleaseAsync(CancellationToken.None);
            }
        }
    }
}
