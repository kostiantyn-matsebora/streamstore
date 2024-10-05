using StreamStore.S3.Client;

namespace StreamStore.S3.Lock
{
    public interface IS3StreamLockFactory
    {
        IS3StreamLock CreateLock(Id streamId, S3StreamLockPolicy policy);

    }
}