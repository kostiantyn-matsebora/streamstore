namespace StreamStore.Exceptions
{
    public class PessimisticConcurrencyException: ConcurrencyControlException
    {
        public PessimisticConcurrencyException(Id streamId) : base(streamId, $"Stream {streamId} is being changed right now.")
        {
        }

        public PessimisticConcurrencyException(Id streamId, string message) : base(streamId, message)
        {
        }
    }
}
