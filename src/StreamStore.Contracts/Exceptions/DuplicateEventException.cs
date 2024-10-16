using System;

namespace StreamStore.Exceptions
{
    public sealed class DuplicateEventException : StreamStoreException
    {
        public Id EventId { get; set; }

        public DuplicateEventException(Id eventId, Id streamId)
                    : base(streamId, "Found duplicated event for stream.")
        {
            EventId = eventId;
        }
    }
}