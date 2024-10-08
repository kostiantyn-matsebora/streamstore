namespace StreamStore.Exceptions
{
    public class PessimisticConcurrencyException: ConcurrencyException
    {
        public PessimisticConcurrencyException(Id streamId) : base($"Stream is being changed right now.")
        {
            StreamId = streamId;
        }

        public Id StreamId { get; }
    }
}
