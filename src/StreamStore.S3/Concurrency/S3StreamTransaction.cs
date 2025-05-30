using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.Exceptions.Appending;
using StreamStore.S3.Client;


namespace StreamStore.S3.Concurrency
{
    internal sealed class S3StreamTransaction : IDisposable, IAsyncDisposable
    {
        readonly S3StreamContext ctx;
        readonly IS3LockFactory factory;

        IS3LockHandle? handle;
        bool commited = false;
        bool rolledBack = false;


        public S3StreamTransaction(S3StreamContext ctx, IS3LockFactory factory)
        {
            this.ctx = ctx;
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<S3StreamTransaction> BeginAsync(CancellationToken token)
        {
            var @lock = factory.CreateLock(ctx.StreamId, ctx.TransactionId);
            handle = await @lock.AcquireAsync(token);

            if (handle == null)
                throw new StreamLockedException(ctx.StreamId);
            return this;
        }

        public async Task CommitAsync(CancellationToken token)
        {
            if (commited || rolledBack)
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            // Copying transient stream to persistent and delete transient
            await ctx.SaveChangesAsync(token);

            // Just releasing lock
            await handle!.ReleaseAsync(token);
            ctx.ResetState();
            commited = true;
        }

        public async Task RollbackAsync() //TODO: Add retry logic
        {
            if (commited) return;
            if (rolledBack) return;
            try
            {
                rolledBack = true;
                await ctx.RollBackAsync(CancellationToken.None);
            }
            finally
            {
                ctx.ResetState();
            }
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

        public static async Task<S3StreamTransaction> BeginAsync(S3StreamContext ctx, IS3LockFactory factory)
        {
            var transaction = new S3StreamTransaction(ctx, factory);
            await transaction.BeginAsync(CancellationToken.None);
            return transaction;
        }
    }
}
