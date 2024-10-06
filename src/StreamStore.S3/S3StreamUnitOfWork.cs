using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;


namespace StreamStore.S3
{
    internal sealed class S3StreamUnitOfWork : IStreamUnitOfWork
    {        
        readonly S3EventRecordCollection records = new S3EventRecordCollection();
        readonly Id id;
        readonly int expectedRevision;
        readonly IS3Factory factory;

        public S3StreamUnitOfWork(Id streamId, int expectedRevision, IS3Factory factory)
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
                new S3EventRecord {
                    Id = eventId,
                    Revision = revision,
                    Timestamp = timestamp,
                    Data = data
            });
            return this;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await using var client = factory.CreateClient();
            if (!records.Any())
                throw new InvalidOperationException("No events to save.");

            await ThrowIfStreamAlreadyChanged(client, cancellationToken);

            int revision = expectedRevision;

            using var transaction = await S3StreamTransaction.BeginAsync(id, factory);
            try
            {
                // Get the current revision
                var existingMetadata = await ThrowIfStreamAlreadyChanged(client, cancellationToken);

                var streamMetadata = S3StreamMetadata.New(id, records.ToEventMetadata());

                // If there is already events for stream
                if (existingMetadata != null)
                    streamMetadata.AddRange(existingMetadata);

                // Create new stream
                var newStream = S3Stream.New(streamMetadata, records);

                // Update stream
                using var updater = S3StreamUpdater.New(newStream, factory.CreateClient());
                await updater.UpdateAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        async Task<S3StreamMetadata?> ThrowIfStreamAlreadyChanged(IS3Client client, CancellationToken token)
        {

            var response = await client.FindObjectAsync(S3Naming.StreamKey(id), token);
            if (response == null) return null;

            var stream = Converter.FromByteArray<S3StreamMetadata>(response.Data!);

            if (stream!.Revision != expectedRevision)
                throw new OptimisticConcurrencyException(expectedRevision, stream!.Revision, id);
            return stream;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
