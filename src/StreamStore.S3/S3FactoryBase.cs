using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.S3.Storage;

namespace StreamStore.S3
{
    internal abstract class S3FactoryBase : IS3LockFactory, IS3ClientFactory, IS3StorageFactory
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
            var fileLock = new S3FileLock(storage.Locks.GetItem(streamId), transactionId);
            return new S3CompositeStreamLock(inMemoryLock, fileLock);
        }

        public IS3Storage CreateStorage()
        {
           return new S3Storage(this);
        }
    }
}
