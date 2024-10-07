using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;

namespace StreamStore.S3
{
    internal sealed class S3StreamTransaction : IDisposable, IAsyncDisposable
    {
        readonly Id streamId;
        readonly IS3Factory factory;
        IS3LockHandle? handle;
        bool commited = false;
        bool rolledBack = false;
        readonly IS3StreamLock @lock;

        public S3Stream? Before { get; private set; }

        private S3StreamTransaction(Id streamId, IS3StreamLock @lock, IS3Factory factory)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));
            this.streamId = streamId;
            this.@lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }



        async Task BeginAsync(CancellationToken token)
        {
            handle = await @lock.AcquireAsync(token);

            if (handle == null)
                throw new StreamAlreadyLockedException(streamId);

            // Get stream metadata first
            await using var client = factory.CreateClient();
            Before = await GetStream(client);
        }


        public async Task CommitAsync(CancellationToken token)
        {
            if (commited || rolledBack)
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            // Just releasing lock
            await handle!.ReleaseAsync(token);
            commited = true;
        }


        public async Task RollbackAsync() //TODO: Add retry logic
        {
            if (commited) return;
            if (rolledBack)
                throw new InvalidOperationException("Transaction already rolled back.");

            try
            {
                await using (var client = factory.CreateClient()) 
                {
                    // First get current stream
                    S3Stream? current = await GetStream(client);

                    // Delete events were added during transaction
                    if (Before == null)
                        await DeleteStreamFully(client);
                    else
                    {
                        //Deleting uncommited events only
                        if (current != null)
                           await DeleteUncommitedEvents(client, current);
                        // Fully restoring events from state before transaction
                        else
                            await RestoreStream(client);
                    }
                }
            }
            catch
            {
                rolledBack = true; // at least we've tried
                throw;
            }
        }

        private async Task RestoreStream(IS3Client client)
        {
            var updater = S3StreamUpdater.New(Before!, client);
            await updater.UpdateAsync(CancellationToken.None);
        }

        private async Task DeleteStreamFully(IS3Client client)
        {
            await client.DeleteObjectAsync(S3Naming.StreamPrefix(streamId), null, CancellationToken.None);
        }

        async Task<S3Stream?> GetStream(IS3Client client)
        {
            var loader = new S3StreamLoader(streamId, client);
            return  await loader.LoadAsync(CancellationToken.None);
        }

        async Task DeleteUncommitedEvents(IS3Client client, S3Stream stream)
        {
            var current = stream.Events.ToEventMetadata();

            var before = Before!.Events.ToEventMetadata();

            var after = current.Except(before);

            foreach (var @event in after)
                await client.DeleteObjectAsync(
                    S3Naming.StreamPrefix(streamId), 
                    S3Naming.EventKey(streamId, @event.Id), 
                    CancellationToken.None);
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
                handle = null;
                Before = null!;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public static async Task<S3StreamTransaction> BeginAsync(Id streamId, IS3Factory factory)
        {
            var transaction = new S3StreamTransaction(streamId, factory.CreateLock(streamId), factory);
            await transaction.BeginAsync(CancellationToken.None);
            return transaction;
        }
    }
}
