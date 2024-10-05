using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;

namespace StreamStore.S3
{
    internal sealed class S3StreamTransaction: IDisposable
    {
        Id streamId;
        IS3Client? client;
        S3AbstractFactory factory;
        IS3StreamLock? streamLock;
        bool commited = false;
        bool rolledBack = false;

        public S3Stream? Stream { get; private set; }

        S3StreamTransaction(Id streamId, S3AbstractFactory factory)
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
            using var streamLoader = new S3StreamLoader(streamId, client!);
            Stream = await streamLoader.LoadAsync(token);
        }


        public async Task CommitAsync(CancellationToken token)
        {
            if (commited || rolledBack)
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            // Just releasing lock
            await streamLock!.ReleaseAsync(token);
        }


        public async Task RollbackAsync()
        {
            if (commited || rolledBack)
                throw new InvalidOperationException("Transaction already commited or rolled back.");

            try
            {
                // First get current stream
                using var loader = factory!.CreateLoader(streamId);
                var stream = await loader.LoadAsync(CancellationToken.None);

                // Delete events were added during transaction
                if (stream != null)
                {
                    var newEventIds =
                        stream.Events.Select(e => e.Id)
                        .Except(Stream!.Events.Select(e => e.Id))
                        .ToList();

                    using var client = factory.CreateClient();
                    foreach (var eventId in newEventIds)
                    {
                        await client.DeleteObjectAsync(S3Naming.EventKey(streamId, eventId), CancellationToken.None);
                    }
                }

                // Then update stream by stored version
                if (Stream != null)
                {
                    using var updater = factory.CreateUpdater(Stream);
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

                client?.Dispose();
                client = null;
                streamLock?.Dispose();
                streamLock = null;
                Stream = null!;

            }
        }

        public static async Task<S3StreamTransaction> BeginAsync(Id streamId, S3AbstractFactory factory)
        {
            var transaction = new S3StreamTransaction(streamId, factory);
            await transaction.BeginAsync(CancellationToken.None);
            return transaction;
        }
    }
}
