using System;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;

namespace StreamStore.S3
{
    internal sealed class S3AbstractFactory
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
            return  lockFactory.CreateLock(streamId);
        }

        public S3StreamLoader CreateLoader(Id streamId)
        {
            return new S3StreamLoader(streamId, CreateClient());
        }

        public S3StreamUpdater CreateUpdater(S3Stream stream)
        {
            return new S3StreamUpdater(stream, CreateClient());
        }

        public S3StreamDeleter CreateDeleter(Id streamId)
        {
            return new S3StreamDeleter(streamId, CreateClient());
        }
    }
}