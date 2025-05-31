namespace StreamStore.Exceptions.Appending
{
    public sealed class EventAlreadyExistsException : OptimisticConcurrencyException
    {
        public Id? EventId { get; set; }

        public EventAlreadyExistsException(Id eventId, Id streamId)
                    : base(streamId, $"Found duplicated event {eventId} for stream.")
        {
            EventId = eventId;
        }

        public EventAlreadyExistsException(Id streamId)
                  : base(streamId, "Found duplicated event for stream.")
        {
        }
    }
}