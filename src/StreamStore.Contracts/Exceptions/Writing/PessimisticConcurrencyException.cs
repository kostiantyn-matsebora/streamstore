namespace StreamStore.Exceptions
{
    public class PessimisticConcurrencyException: ConcurrencyException
    {
        public PessimisticConcurrencyException(Id streamId) : base(streamId, $"Stream {streamId} is being changed right now.")
        {
        }
    }
}
