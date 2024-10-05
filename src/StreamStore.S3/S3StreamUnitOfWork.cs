using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Amazon.S3.Model;
using StreamStore.S3.Client;
using StreamStore.S3.Models;


namespace StreamStore.S3
{
    internal sealed class S3StreamUnitOfWork : IStreamUnitOfWork
    {        
        readonly List<EventRecord> records = new List<EventRecord>();
        readonly Id id;
        readonly int expectedRevision;
        readonly S3AbstractFactory factory;

        public S3StreamUnitOfWork(Id streamId, int expectedRevision,  S3AbstractFactory factory)
        {

            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId));
            id = streamId;
            if (expectedRevision < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedRevision));
            this.expectedRevision = expectedRevision;
            
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IStreamUnitOfWork Add(Id eventId, int revision, DateTime timestamp, string data)
        {
            records.Add(
                new EventRecord {
                    Id = eventId,
                    Revision = revision,
                    Timestamp = timestamp,
                    Data = data
            });
            return this;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            using var client = factory.CreateClient();

            if (records.Count == 0)
                throw new InvalidOperationException("No events to save.");

            await ThrowIfStreamAlreadyChanged(client, cancellationToken);

            int revision = expectedRevision;

            using (var transaction = await S3StreamTransaction.BeginAsync(id, factory))
            {
                try
                {
                    // Get the current revision
                    var stream = await ThrowIfStreamAlreadyChanged(client, cancellationToken);

                    var ids = new List<string>();

                    // If there is already events for stream
                    var mergedIds = stream != null ? ids.Concat(stream!.EventIds) : ids;

                    // Create new stream
                    var newStream = new S3Stream(S3StreamMetadata.New(id, revision, mergedIds), records);

                    // Update stream
                    using var updater =factory.CreateUpdater(newStream);
                    await updater.UpdateAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync();
                }
            }
        }

        async Task<S3StreamMetadata?> ThrowIfStreamAlreadyChanged(IS3Client client, CancellationToken token)
        {

            var data = await client.FindObjectAsync(S3Naming.StreamKey(id), token);
            if (data == null)
                return null;
            var stream = Converter.FromByteArray<S3StreamMetadata>(data);

            if (stream!.Revision != expectedRevision)
                throw new OptimisticConcurrencyException(expectedRevision, stream!.Revision, id);
            return stream;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            // There is no IDisposable objects to dispose
        }

    }
}
