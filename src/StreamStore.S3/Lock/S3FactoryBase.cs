using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;

namespace StreamStore.S3.Lock
{
    public abstract class S3FactoryBase : IS3LockFactory, IS3ClientFactory
    {
        readonly S3Storage storage;
        readonly S3InMemoryStreamLockStorage lockStorage;

        protected S3FactoryBase()
        {
            storage = new S3Storage(this);
            lockStorage = new S3InMemoryStreamLockStorage();
        }
        

      

        public abstract IS3Client CreateClient();

        public IS3StreamLock CreateLock(Id streamId, Id transactionId)
        {
            var inMemoryLock = new S3StreamInMemoryLock(streamId, lockStorage);
            var fileLock = new S3FileLock(storage.Locks.GetChild(streamId), transactionId);
            return new S3CompositeStreamLock(inMemoryLock, fileLock);
        }
    }
}
