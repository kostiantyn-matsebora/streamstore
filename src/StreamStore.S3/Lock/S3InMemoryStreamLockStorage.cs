using System.Collections.Concurrent;

namespace StreamStore.S3.Lock
{
    class S3InMemoryStreamLockStorage
    {
        readonly ConcurrentDictionary<Id, LockId> locks = new ConcurrentDictionary<Id, LockId>();

        public LockId? TryLock(Id streamId)
        {
            if (locks.ContainsKey(streamId)) return null;

            var lockId = new LockId();
            if (locks.TryAdd(streamId, lockId)) return lockId;
            return null;
        }

        public void ReleaseLock(Id streamId)
        {
            locks.TryRemove(streamId, out _);
        }
    }
}
