using System;

namespace StreamStore
{
    [Serializable]
    public sealed class DuplicateEventException : StreamStoreException
    {
        public Id EventId { get; set; }
        public Id StreamId { get; set; }

        public DuplicateEventException(Id eventId, Id streamId)
                    : base("Found duplicated event for stream.")
        {
            EventId = eventId;
            StreamId = streamId;
        }
    }
}