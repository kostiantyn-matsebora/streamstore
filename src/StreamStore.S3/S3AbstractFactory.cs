using System;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3
{
    public sealed class S3AbstractFactory
    {
        readonly IS3ClientFactory clientFactory;
        readonly IS3StreamLockFactory lockFactory;

        public S3AbstractFactory(IS3ClientFactory clientFactory, IS3StreamLockFactory lockFactory)
        {
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            this.lockFactory = lockFactory ?? throw new ArgumentNullException(nameof(lockFactory));
        }
       
        public IS3Client CreateClient()
        {
            return clientFactory.CreateClient();
        }


        public IS3StreamLock CreateLock(Id streamId)
        {
            return lockFactory.CreateLock(streamId);
        }
    }
}