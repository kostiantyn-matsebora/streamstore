using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;


namespace StreamStore.S3
{
    internal sealed class S3StreamDatabase : IStreamDatabase
    {
        readonly IS3LockFactory lockFactory;
        private readonly IS3StorageFactory storageFactory;

        public S3StreamDatabase(IS3LockFactory lockFactory, IS3StorageFactory storageFactory)
        {
            this.lockFactory = lockFactory ?? throw new ArgumentNullException(nameof(lockFactory));
            this.storageFactory = storageFactory ?? throw new ArgumentNullException(nameof(storageFactory));
        }

        public async Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            var context = new S3StreamContext(streamId, expectedStreamVersion, storageFactory.CreateStorage());
            await context.Initialize(token);
            return new S3StreamUnitOfWork(lockFactory, context);
        }

        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            await TryDeleteAsync(streamId, token);
        }

        public async Task<EventMetadataRecordCollection?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            var storage = storageFactory.CreateStorage();

            var metadata = await storage.Persistent.LoadMetadataAsync(streamId);

            if (metadata!.State == S3ObjectState.DoesNotExist) return null;

            return metadata.Events;
        }

        async Task TryDeleteAsync(Id streamId, CancellationToken token)
        {
            var storage = storageFactory.CreateStorage();

            await storage.Persistent.DeleteContainerAsync(streamId, token);
        }

        public Task<EventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}