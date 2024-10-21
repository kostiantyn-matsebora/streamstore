using System;


namespace StreamStore
{
    public sealed class EventEntity: IHasRevision {
        public Id EventId { get; }
        public DateTime Timestamp { get; }
        public int Revision { get; }
        public object Event { get; internal set; }

        public EventEntity(Id id, Revision revision, DateTime timestamp, object @event)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException(nameof(id), "Id cannot be empty.");
            if (timestamp == default)
                throw new ArgumentOutOfRangeException(nameof(timestamp));
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            EventId = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event;
        }
    }
}
