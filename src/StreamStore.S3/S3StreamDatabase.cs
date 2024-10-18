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
        private readonly S3Storage storage;

        public S3StreamDatabase(IS3LockFactory lockFactory, S3Storage storage)
        {
            this.lockFactory = lockFactory ?? throw new ArgumentNullException(nameof(lockFactory));
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            this.storage = storage;
        }

        public Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult((IStreamUnitOfWork)new S3StreamUnitOfWork(lockFactory, new S3StreamContext(streamId, expectedStreamVersion, storage)));
        }

        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            await TryDeleteAsync(streamId, token);
        }

         public async Task<StreamRecord?> FindAsync(Id streamId, CancellationToken token = default)
        {
            var container = await storage.Persistent.LoadAsync(streamId, token);
          
            if (container.State == S3ObjectState.NotExists) return null;

            return new StreamRecord(container.Events.Select(e => e.Event!));
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            var metadata = await storage.Persistent.LoadMetadataAsync(streamId);
            if (metadata!.State == S3ObjectState.NotExists) return null;

            return new StreamMetadataRecord(metadata.Metadata!.Events!);
        }

        async Task TryDeleteAsync(Id streamId, CancellationToken token)
        {
            await storage.Persistent.DeleteContainerAsync(streamId, token);
        }
    }
}