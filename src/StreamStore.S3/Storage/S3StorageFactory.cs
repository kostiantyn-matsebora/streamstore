using System;
using StreamStore.S3.Client;

namespace StreamStore.S3.Storage
{
    internal class S3StorageFactory: IS3StorageFactory
    {
        readonly IS3ClientFactory clientFactory;

        public S3StorageFactory(IS3ClientFactory clientFactory)
        {
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public IS3Storage CreateStorage()
        {
           return new S3Storage(clientFactory);
        }
    }
}
