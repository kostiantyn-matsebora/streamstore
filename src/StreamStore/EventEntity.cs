using System;


namespace StreamStore
{
    public sealed class EventEntity {
        public string EventId { get; }
        public DateTime Timestamp { get; }
        public int Revision { get; }
        public object Event { get; internal set; }

        public EventEntity(Id id, int revision, DateTime timestamp, object @event)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException("Id cannot be empty.", nameof(id));
            if (timestamp == default)
                throw new ArgumentOutOfRangeException(nameof(timestamp));
            if (revision < 1)
                throw new ArgumentOutOfRangeException("Revision cannot be less than 1", nameof(revision));
            if (@event == null) 
                throw new ArgumentNullException(nameof(@event));
            EventId = id;
            Timestamp = timestamp;
            Revision = revision;
            Event = @event;
        }
    }
}
