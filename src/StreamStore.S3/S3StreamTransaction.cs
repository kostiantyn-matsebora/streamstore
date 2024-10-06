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

        public S3StreamMetadata? Before { get; private set; }

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
            using var streamLoader = new S3StreamLoader(streamId, factory.CreateClient());
            Before = (await streamLoader.LoadAsync(token))?.Metadata;
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
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            try
            {
                // First get current stream
                await using var client = factory.CreateClient();
                S3Stream? stream = null;
                using (var loader = new S3StreamLoader(streamId, factory.CreateClient()))
                    stream = await loader.LoadAsync(CancellationToken.None);

                // Delete events were added during transaction
                if (stream != null)
                {
                    var current = stream.Events.ToEventMetadata();

                    var after = Before != null ? current.Except(Before!.Events) : current;
                    foreach (var eventMetadata in after)
                    {
                        await client.DeleteObjectAsync(S3Naming.EventKey(streamId, eventMetadata.Id), CancellationToken.None);
                    }
                }

                // Then update stream metadata by stored version
                if (Before != null)
                {
                    var onlyMetadata = S3Stream.New(Before, new S3EventRecordCollection(Array.Empty<S3EventRecord>()));
                    using var updater = S3StreamUpdater.New(onlyMetadata, factory.CreateClient());
                    await updater.UpdateAsync(CancellationToken.None);
                }
            }
            catch
            {
                rolledBack = true; // at least we've tried
                throw;
            }
        }

        public void Dispose()
        {
            DisposeAsync(true).ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (!commited && !rolledBack) await RollbackAsync();

                handle?.DisposeAsync();
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
