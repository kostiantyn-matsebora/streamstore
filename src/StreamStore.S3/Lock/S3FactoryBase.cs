using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;

namespace StreamStore.S3.Lock
{
    public abstract class S3FactoryBase : IS3Factory
    {
        readonly S3InMemoryStreamLockStorage storage = new S3InMemoryStreamLockStorage();

        public abstract IS3Client CreateClient();

        public IS3StreamLock CreateLock(IS3TransactionContext ctx)
        {
            var inMemoryLock = new S3StreamInMemoryLock(ctx.StreamId, storage);
            var fileLock = new S3FileLock(ctx, this);
            return new S3CompositeStreamLock(inMemoryLock, fileLock);
        }
    }
}
