using System;

namespace StreamDB
{
    [Serializable]
    public sealed class DuplicateEventException : StreamDBException
    {
        public string[] EventIds { get; set; }
        public Id StreamId { get; set; }

        public DuplicateEventException(string[] eventIds, Id streamId)
                    : base("Found duplicated events for stream.")
        {
            EventIds = eventIds;
            StreamId = streamId;
        }
    }
}