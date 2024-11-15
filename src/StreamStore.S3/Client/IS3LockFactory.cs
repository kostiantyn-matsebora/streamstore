
namespace StreamStore.S3.Client
{
    public interface IS3LockFactory
    {
        IS3StreamLock CreateLock(Id streamId, Id transactionId);
    }


    public interface IS3ClientFactory
    {
        IS3Client CreateClient();
    }
}
