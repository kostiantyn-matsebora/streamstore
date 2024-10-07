using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace StreamStore.S3.Lock
{
    class S3InMemoryStreamLockStorage
    {
        readonly MemoryCache locks = new MemoryCache(new MemoryCacheOptions());
        readonly TimeSpan ttl;
        readonly SemaphoreSlim semaphore;

        public S3InMemoryStreamLockStorage(TimeSpan ttl, int parallelCount = 1)
        {
            if (ttl == TimeSpan.Zero) 
                throw new ArgumentException("TTL must be set.", nameof(ttl));
            this.ttl = ttl;
            if (parallelCount <= 0)
                throw new ArgumentException("Parallel count must be greater than 0.", nameof(parallelCount));
            semaphore = new SemaphoreSlim(parallelCount);
        }

        public LockId? TryAdd(Id streamId)
        {
            semaphore.Wait();
            try
            {
                locks.TryGetValue(streamId, out LockId? lockId);
                if (lockId != null) return null;
                
                lockId = new LockId();
                
                return  locks.Set(streamId, lockId,
                    new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow  = ttl }
                );
                
            } finally
            {
                semaphore.Release();
            }
        }

        public void TryRemove(Id streamId)
        {
            locks.Remove(streamId);
        }
    }
}
