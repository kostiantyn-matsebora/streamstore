namespace StreamStore.Exceptions.Appending
{
    public sealed class DuplicatedEventException: ValidationException
    {
        public Id? EventId { get; set; }

        public DuplicatedEventException(Id eventId, Id streamId)
                    : base(streamId, $"Found duplicated event {eventId} for stream.")
        {
            EventId = eventId;
        }
    }
}