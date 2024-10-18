using StreamStore.S3.Client;

namespace StreamStore.S3.Storage
{
    internal class S3LockStorage : S3ObjectStorage<S3LockObject, S3ObjectContainer>
    {
        public S3LockStorage(S3ContainerPath parent, IS3ClientFactory clientFactory) : base(parent, clientFactory)
        {
        }

        protected override S3ObjectContainer CreateContainer(string name)
        {
            return new S3ObjectContainer(path.Combine(name), clientFactory);
        }

        protected override S3LockObject CreateItem(string name)
        {
            return new S3LockObject(path.Combine(name), clientFactory);
        }
    }
}
