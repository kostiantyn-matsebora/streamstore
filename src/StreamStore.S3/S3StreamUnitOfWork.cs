using System;

using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;


namespace StreamStore.S3
{
    internal sealed class S3StreamUnitOfWork : IStreamUnitOfWork
    {
        readonly int expectedRevision;
        readonly IS3Factory factory;
        readonly S3TransactionContext ctx;
        int revision;

        public S3StreamUnitOfWork(Id streamId, int expectedRevision, IS3Factory factory)
        {
            if (streamId == Id.None)
                throw new ArgumentNullException(nameof(streamId));
          
            if (expectedRevision < 0)
                throw new ArgumentOutOfRangeException(nameof(expectedRevision));
            this.expectedRevision = expectedRevision;
            
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            ctx = S3TransactionContext.New(streamId);
            revision = expectedRevision;
        }

        public IStreamUnitOfWork Add(Id eventId, DateTime timestamp, string data)
        {
            ctx.Add(
                new EventRecord {
                    Id = eventId,
                    Revision = ++revision,
                    Timestamp = timestamp,
                    Data = data
            });
            return this;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await using var client = factory.CreateClient();
            if (!ctx.HasChanges)
                throw new InvalidOperationException("No events to save.");

            var metadata = await GetStreamMetadata(client, cancellationToken);

            ThrowIfStreamAlreadyChanged(
                await GetStreamMetadata(client, cancellationToken));

            using var transaction = await S3StreamTransaction.BeginAsync(ctx, factory);
            try
            {
                // Get the current revision
                metadata = await GetStreamMetadata(client, cancellationToken);
                ThrowIfStreamAlreadyChanged(metadata);

                if (metadata == null)
                    metadata = S3StreamMetadata.New(ctx.StreamId, ctx.UncommitedMetadata);
                else 
                    metadata.AddRange(ctx.UncommitedMetadata);

                // Update stream
                await S3StreamUploader
                    .New(ctx.Transient, client)
                    .UploadAsync(metadata, ctx.Uncommited, cancellationToken);

                // Commit transaction
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        async Task<S3StreamMetadata?> GetStreamMetadata(IS3Client client, CancellationToken token)
        {
            var response = await client.FindObjectAsync(ctx.Persistent.MetadataKey, token);
            if (response == null) return null;

            var record = Converter.FromByteArray<S3StreamMetadataRecord>(response.Data!);
            return record!.ToMetadata();
        }

        void ThrowIfStreamAlreadyChanged(S3StreamMetadata? stream)
        {
            if (stream == null)
                return;
            if (stream!.Revision != expectedRevision)
                throw new OptimisticConcurrencyException(expectedRevision, stream!.Revision, ctx.StreamId);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
