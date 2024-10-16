using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.S3.Client;
using StreamStore.S3.Operations;

namespace StreamStore.S3.Concurrency
{
    internal sealed class S3StreamTransaction : IDisposable, IAsyncDisposable
    {
        readonly S3TransactionContext ctx;
        readonly IS3Factory factory;

        IS3LockHandle? handle;
        bool commited = false;
        bool rolledBack = false;


        private S3StreamTransaction(S3TransactionContext ctx, IS3Factory factory)
        {
            this.ctx = ctx;
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        async Task BeginAsync(CancellationToken token)
        {
            var @lock = factory.CreateLock(ctx);
            handle = await @lock.AcquireAsync(token);

            if (handle == null)
                throw new StreamLockedException(ctx.StreamId);
        }

        public async Task CommitAsync(CancellationToken token)
        {
            if (commited || rolledBack)
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            // Copying transient stream to persistent
            await ApplyStreamChanges(token);

            // Delete transient stream
            await DeleteTransient(token);

            // Just releasing lock
            await handle!.ReleaseAsync(token);
            commited = true;
        }

        async Task ApplyStreamChanges(CancellationToken token)
        {
            await using (var client = factory.CreateClient())
                await S3StreamCopier
                        .New(ctx.Transient, ctx.Persistent, client)
                        .CopyAsync(token);
        }

        public async Task RollbackAsync() //TODO: Add retry logic
        {
            if (commited) return;
            if (rolledBack) return;

            rolledBack = true;
            await DeleteTransient(CancellationToken.None);
        }

        async Task DeleteTransient(CancellationToken token)
        {
            await using (var client = factory.CreateClient())
                await S3StreamDeleter
                         .New(ctx.Transient, client)
                         .DeleteAsync(token);
        }

        public void Dispose()
        {
            DisposeAsync(true).GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }

        async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (!commited && !rolledBack) await RollbackAsync();
                if (handle != null) await handle.DisposeAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public static async Task<S3StreamTransaction> BeginAsync(S3TransactionContext ctx, IS3Factory factory)
        {
            var transaction = new S3StreamTransaction(ctx, factory);
            await transaction.BeginAsync(CancellationToken.None);
            return transaction;
        }
    }
}
