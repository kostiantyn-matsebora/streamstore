using StreamStore.S3.Client;
using StreamStore.S3.Lock;


namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemoryFactory : IS3Factory
    {
        public IS3Client CreateClient()
        {
            return new S3InMemoryClient();
        }

        public IS3StreamLock CreateLock(Id streamId)
        {
            return new S3StreamInMemoryLock(streamId, new S3InMemoryStreamLockStorage(TimeSpan.FromSeconds(10)));
        }
    }
}