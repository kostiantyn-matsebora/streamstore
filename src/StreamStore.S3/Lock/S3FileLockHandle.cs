using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;


namespace StreamStore.S3.Lock
{
    class S3FileLockHandle : IS3LockHandle
    {
        bool lockReleased = false;
        readonly IS3TransactionContext ctx;
        string? fileId;
        readonly IS3Factory? factory;

        public S3FileLockHandle(IS3TransactionContext ctx, string fileId, IS3Factory factory)
        {

            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            this.fileId = fileId;
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            if (!lockReleased)
            {
                lockReleased = true;
                await using var client = factory!.CreateClient();
                await client!.DeleteObjectByFileIdAsync(fileId!, ctx.LockKey, token);
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
                fileId = null;
            }
        }
    }
}
