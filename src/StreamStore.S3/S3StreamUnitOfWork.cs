using System;

using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;


namespace StreamStore.S3
{
    internal sealed class S3StreamUnitOfWork : StreamUnitOfWorkBase
    {
      
        readonly IS3Factory factory;
        readonly S3TransactionContext ctx;
        public S3StreamUnitOfWork(Id streamId, Revision expectedRevision, IS3Factory factory): base(streamId, expectedRevision, null)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            ctx = S3TransactionContext.New(streamId);
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            if (!ctx.HasChanges)
                throw new InvalidOperationException("No events to save.");

            await using var client = factory.CreateClient();
          
            ThrowIfStreamAlreadyChanged(await GetStreamMetadata(client, token));

            using var transaction = await S3StreamTransaction.BeginAsync(ctx, factory);
            try
            {
                // Get the current revision
                var metadata = await GetStreamMetadata(client, token);
                ThrowIfStreamAlreadyChanged(metadata);

                if (metadata == null)
                    metadata = S3StreamMetadata.New(ctx.StreamId, ctx.UncommitedMetadata);
                else
                    metadata.AddRange(ctx.UncommitedMetadata);

                // Update stream
                await S3StreamUploader
                    .New(ctx.Transient, client)
                    .UploadAsync(metadata, ctx.Uncommited, token);

                // Commit transaction
                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected override Task OnEventAdded(EventRecord @event, CancellationToken token)
        {
            ctx.Add(@event);
            return Task.CompletedTask;
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
            if (stream == null) return;
            if (stream!.Revision != expectedRevision)
                throw new OptimisticConcurrencyException(expectedRevision, stream!.Revision, ctx.StreamId);
        }
    }
}
