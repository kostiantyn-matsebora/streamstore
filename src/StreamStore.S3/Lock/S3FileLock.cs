﻿using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;


namespace StreamStore.S3.Lock
{
    partial class S3FileLock : IS3StreamLock
    {
        private readonly IS3Object lockObject;
        private readonly Id transactionId;

        public S3FileLock(IS3Object lockObject, Id transactionId)
        {
            this.lockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject)); 
            if (!transactionId.HasValue()) throw new ArgumentException("Transaction id is not set.", nameof(transactionId));

            this.transactionId = transactionId;
        }

        public async Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {
            var lockId = Guid.NewGuid().ToString();

            // Trying to figure out if lock already acquired
            await lockObject.LoadAsync(token);
            if (lockObject.State == S3ObjectState.Loaded) return null;

            lockObject.Data = Converter.ToByteArray(new LockId(transactionId));
            await lockObject.UploadAsync(token);

            // Trying to figure out if lock already acquired by someone else
            await lockObject.LoadAsync(token);
            if (lockObject.State == S3ObjectState.NotExists) return null;
            

            var existingLockId = Converter.FromByteArray<LockId>(lockObject.Data!);

            if (existingLockId == null) return null;

            if (!existingLockId!.Equals(transactionId)) return null;

            return new S3FileLockHandle(lockObject);
        }
    }
}
