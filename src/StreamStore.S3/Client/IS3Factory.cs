using StreamStore.S3.Concurrency;

namespace StreamStore.S3.Client
{
    public interface IS3Factory
    {
        IS3Client CreateClient();
        IS3StreamLock CreateLock(IS3TransactionContext ctx);
    }

}
