using System;

namespace StreamStore
{
    [Serializable]
    public sealed class DuplicateEventException : StreamStoreException
    {
        public Id[] EventIds { get; set; }
        public Id StreamId { get; set; }

        public DuplicateEventException(Id[] eventIds, Id streamId)
                    : base("Found duplicated events for stream.")
        {
            EventIds = eventIds;
            StreamId = streamId;
        }
    }
}