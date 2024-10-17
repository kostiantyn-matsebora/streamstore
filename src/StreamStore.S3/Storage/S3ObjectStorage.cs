using System.Collections.Generic;
using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal abstract class S3ObjectStorage<T> : S3ObjectContainer<T> where T : S3Object
    {
        protected S3ObjectStorage(S3ContainerPath parent, IS3ClientFactory clientFactory) : base(parent, clientFactory)
        {
        }
    }

    internal class S3ObjectStorage : S3ObjectStorage<S3Object>
    {
        public S3ObjectStorage(S3ContainerPath parent, IS3ClientFactory clientFactory) : base(parent, clientFactory)
        {
        }

        public override S3Object CreateChild(string name)
        {
            return new S3Object(path.Combine(name), clientFactory);
        }
    }
}
