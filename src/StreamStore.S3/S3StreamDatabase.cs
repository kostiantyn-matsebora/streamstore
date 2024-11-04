using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Database;
using StreamStore.Exceptions;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;


namespace StreamStore.S3
{
    internal sealed class S3StreamDatabase : StreamDatabaseBase
    {
        readonly IS3LockFactory lockFactory;
        private readonly IS3StorageFactory storageFactory;

        public S3StreamDatabase(IS3LockFactory lockFactory, IS3StorageFactory storageFactory)
        {
            this.lockFactory = lockFactory ?? throw new ArgumentNullException(nameof(lockFactory));
            this.storageFactory = storageFactory ?? throw new ArgumentNullException(nameof(storageFactory));
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            var storage = storageFactory.CreateStorage();
            var container = await storage.Persistent.LoadAsync(streamId, startFrom, count, token);
            if (container.State == S3ObjectState.DoesNotExist) throw new StreamNotFoundException(streamId);
            return container.Events!.Select(e => e.Event!).ToArray();
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            var storage = storageFactory.CreateStorage();

            await storage.Persistent.DeleteContainerAsync(streamId, token);
        }

        protected override async Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            var context = new S3StreamContext(streamId, expectedStreamVersion, storageFactory.CreateStorage());
            await context.Initialize(token);
            return new S3StreamUnitOfWork(lockFactory, context);
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            var storage = storageFactory.CreateStorage();

            var metadata = await storage.Persistent.LoadMetadataAsync(streamId);

            if (metadata!.State == S3ObjectState.DoesNotExist) return null;

            return metadata.Events;
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            var metadata = await FindMetadataAsyncInternal(streamId, token);
            return metadata?.MaxRevision ?? 0;
        }
    }
}