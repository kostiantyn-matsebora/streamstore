namespace StreamStore.Exceptions
{
    public class StreamLockedException: PessimisticConcurrencyException
    {
        public StreamLockedException(Id streamId) : base(streamId, $"Stream {streamId} is locked for writing.")
        {
        }
        public StreamLockedException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
