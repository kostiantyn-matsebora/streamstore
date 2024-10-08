namespace StreamStore.S3.Concurrency
{
    public interface IS3TransactionContext
    {
        public Id StreamId { get; }
        public Id TransactionId { get; }
        public string LockKey { get; }
    }
}
