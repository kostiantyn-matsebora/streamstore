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

        public IS3StreamLock CreateLock(IS3TransactionContext ctx)
        {
            return new S3StreamInMemoryLock(ctx.StreamId, new S3InMemoryStreamLockStorage(TimeSpan.FromSeconds(10)));
        }
    }
}