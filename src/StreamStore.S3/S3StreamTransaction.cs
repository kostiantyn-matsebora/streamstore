using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;

namespace StreamStore.S3
{
    internal sealed class S3StreamTransaction: IDisposable
    {
        Id streamId;
        IS3Factory factory;
        IS3StreamLock? streamLock;
        bool commited = false;
        bool rolledBack = false;

        public S3StreamMetadata? Before { get; private set; }

        S3StreamTransaction(Id streamId, IS3Factory factory)
        {
            this.streamId = streamId;
            this.factory = factory;
        }

        async Task BeginAsync(CancellationToken token)
        {
            streamLock = factory!.CreateLock(streamId);
            if (!(await streamLock.AcquireAsync(token)))
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
            await streamLock!.ReleaseAsync(token);
            commited = true;
        }


        public async Task RollbackAsync()
        {
            if (commited)
                return;
            if (rolledBack)
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            try
            {
                // First get current stream
                using var client = factory.CreateClient();
                using var loader = new S3StreamLoader(streamId, factory.CreateClient());
                var stream = await loader.LoadAsync(CancellationToken.None);

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
            } catch {
                rolledBack = true; // at least we've tried
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!commited && !rolledBack) 
                    RollbackAsync().Wait();
                streamLock?.Dispose();
                streamLock = null;
                Before = null!;

            }
        }

        public static async Task<S3StreamTransaction> BeginAsync(Id streamId, IS3Factory factory)
        {
            var transaction = new S3StreamTransaction(streamId, factory);
            await transaction.BeginAsync(CancellationToken.None);
            return transaction;
        }
    }
}
