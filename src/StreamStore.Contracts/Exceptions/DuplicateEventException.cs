
namespace StreamStore.Exceptions
{
    public sealed class DuplicateEventException : ConcurrencyException
    {
        public Id? EventId { get; set; }

        public DuplicateEventException(Id eventId, Id streamId)
                    : base(streamId, $"Found duplicated event {eventId} for stream.")
        {
            EventId = eventId;
        }

        public DuplicateEventException(Id streamId)
                  : base(streamId, "Found duplicated event for stream.")
        {
        }
    }
}