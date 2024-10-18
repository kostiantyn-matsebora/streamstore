using System;

using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;


namespace StreamStore.S3
{
    internal sealed class S3StreamUnitOfWork : StreamUnitOfWorkBase
    {
      
        readonly IS3LockFactory factory;
        readonly S3StreamContext streamContext;

        public S3StreamUnitOfWork(IS3LockFactory factory, S3StreamContext streamContext):
            base(streamContext.StreamId, streamContext.ExpectedRevision, null)
        {
           this.streamContext = streamContext;
           this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
         }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            if (!streamContext.NotEmpty)
                throw new InvalidOperationException("No events to save.");

            ThrowIfStreamAlreadyChanged(await streamContext.GetPersistentMetadataAsync(token));

            using var transaction = await new S3StreamTransaction(streamContext, factory).BeginAsync(token);
            try
            {
                // Get the current revision
                var metadata = await streamContext.GetPersistentMetadataAsync(token);

                ThrowIfStreamAlreadyChanged(metadata);

                // Commit transaction
                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected override async Task OnEventAdded(EventRecord @event, CancellationToken token)
        {
            await streamContext.AddTransientEventAsync(@event, token);
        }

        void ThrowIfStreamAlreadyChanged(EventMetadataRecordCollection? stream)
        {
            if (stream == null) return;
            if (stream!.MaxRevision > expectedRevision)
                throw new OptimisticConcurrencyException(expectedRevision, stream!.MaxRevision, streamContext.StreamId);
        }
    }
}
