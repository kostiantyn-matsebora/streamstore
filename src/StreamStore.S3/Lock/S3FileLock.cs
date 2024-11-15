using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Serialization;


namespace StreamStore.S3.Lock
{
    partial class S3FileLock : IS3StreamLock
    {
        private readonly S3LockObject lockObject;
        private readonly Id transactionId;

        public S3FileLock(S3LockObject lockObject, Id transactionId)
        {
            this.lockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject));
            this.transactionId = transactionId.ThrowIfHasNoValue(nameof(transactionId));
        }

        public async Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {
            // Trying to figure out if lock already acquired
            await lockObject.LoadAsync(token);

            if (lockObject.State == S3ObjectState.Loaded) return null;

            lockObject.ReplaceBy(new LockId(transactionId));

            await lockObject.UploadAsync(token);

            // Trying to figure out if lock already acquired by someone else
            await lockObject.LoadAsync(token);
            if (lockObject.State == S3ObjectState.DoesNotExist) return null;
            

            var existingLockId = lockObject.LockId!;

            if (existingLockId == null) return null;

            if (!existingLockId!.Equals(transactionId)) return null;

            return new S3FileLockHandle(lockObject);
        }
    }
}
