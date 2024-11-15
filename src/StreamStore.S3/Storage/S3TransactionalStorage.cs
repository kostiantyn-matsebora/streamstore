using System.Threading.Tasks;
using System.Threading;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3TransactionalStorage: IS3TransactionalStorage
    {
        readonly S3StreamStorage persistent;
        readonly S3StreamStorage transient;
        readonly S3LockStorage locks;

        public S3TransactionalStorage(IS3ClientFactory factory, S3ContainerPath? root = null)
        {
            var rootPath = root ?? S3ContainerPath.Root;
            persistent = new S3StreamStorage(rootPath.Combine("persistent-streams"), factory);
            transient = new S3StreamStorage(rootPath.Combine("transient-streams"), factory);
            locks = new S3LockStorage(rootPath.Combine("locks"), factory);
        }

        public async Task<S3MetadataObject> LoadPersistentMetadataAsync(Id streamId, CancellationToken token = default)
        {
           return await persistent.LoadMetadataAsync(streamId, token);
        }

        public async Task DeletePersistentContainerAsync(Id streamId, CancellationToken token = default)
        {
            await persistent.DeleteContainerAsync(streamId, token);
        }

        public async Task<S3StreamContainer> LoadPersistentContainerAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
          return await persistent.LoadAsync(streamId, startFrom, count, token);
        }

        public S3StreamContainer GetPersistentContainer(string name)
        {
            return persistent.GetContainer(name);
        }

        public S3StreamContainer GetTransientContainer(string name)
        {
            return transient.GetContainer(name);
        }

        public S3LockObject GetLock(string name)
        {
            return  locks.GetItem(name);
        }
    }
}
