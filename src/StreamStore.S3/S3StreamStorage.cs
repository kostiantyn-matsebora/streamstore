using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Storage;
using StreamStore.Exceptions;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;
using System.Collections;
using System.Collections.Generic;


namespace StreamStore.S3
{
    internal sealed class S3StreamStorage : StreamStorageBase<IStreamEventRecord>
    {
        readonly IS3LockFactory lockFactory;
        private readonly IS3StorageFactory storageFactory;

        public S3StreamStorage(IS3LockFactory lockFactory, IS3StorageFactory storageFactory)
        {
            this.lockFactory = lockFactory ?? throw new ArgumentNullException(nameof(lockFactory));
            this.storageFactory = storageFactory ?? throw new ArgumentNullException(nameof(storageFactory));
        }

        protected override async Task<IStreamEventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            var storage = storageFactory.CreateStorage();
            var container = await storage.LoadPersistentContainerAsync(streamId, startFrom, count, token);
            if (container.State == S3ObjectState.DoesNotExist) throw new StreamNotFoundException(streamId);
            return container.Events!.Select(e => e.Event!).ToArray();
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            var storage = storageFactory.CreateStorage();

            await storage.DeletePersistentContainerAsync(streamId, token);
        }

        protected override async Task<IStreamMetadata?> GetMetadataInternal(Id streamId, CancellationToken token = default)
        {

            var storage = storageFactory.CreateStorage();

            var metadata = await storage.LoadPersistentMetadataAsync(streamId);

            if (metadata!.State == S3ObjectState.DoesNotExist) return null;

            var events = new StreamEventMetadataRecordCollection(metadata.Events);

            return new StreamMetadata(streamId, events.MaxRevision, events.LastModified.GetValueOrDefault());

        }

        protected override void BuildRecord(IStreamEventRecordBuilder builder, IStreamEventRecord entity)
        {
            builder.WithRecord(entity);
        }

        protected override async Task WriteAsyncInternal(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
        {
            var streamContext = new S3StreamContext(streamId, batch.MinRevision().Previous(), storageFactory.CreateStorage());


            // Add events to transient storage
            foreach (var record in batch)
            {
                await streamContext.AddTransientEventAsync(record, token);
            }

            if (!streamContext.NotEmpty)
                throw new InvalidOperationException("No events to save.");

            ThrowIfStreamAlreadyChanged(batch.MinRevision(), await streamContext.GetPersistentMetadataAsync(token), streamContext);

            using var transaction = await new S3StreamTransaction(streamContext, lockFactory).BeginAsync(token);

            try
            {
                // Get the current revision
                var metadata = await streamContext.GetPersistentMetadataAsync(token);

                ThrowIfStreamAlreadyChanged(batch.MinRevision(), metadata, streamContext);

                // Commit transaction
                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        static void ThrowIfStreamAlreadyChanged(int minRevision, StreamEventMetadataRecordCollection? stream, S3StreamContext streamContext)
        {
            if (stream == null) return;
            if (stream!.MaxRevision >= minRevision)
                throw new StreamAlreadyMutatedException(minRevision, stream!.MaxRevision, streamContext.StreamId);
        }
    }
}