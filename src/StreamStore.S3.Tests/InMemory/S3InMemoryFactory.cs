using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Lock;


namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemoryFactory : IS3ClientFactory, IS3LockFactory
    {
        public IS3Client CreateClient()
        {
            return new S3InMemoryClient();
        }

        public IS3StreamLock CreateLock(Id streamId, Id transactionId)
        {
            return new S3StreamInMemoryLock(streamId, new S3InMemoryStreamLockStorage(TimeSpan.FromSeconds(10)));
        }
    }
}