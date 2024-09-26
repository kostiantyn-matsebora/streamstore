using System;
namespace StreamDB
{
    public class UncommitedEvent : IUncommitedEvent
    {
        public Id Id { get; internal set; }

        public DateTime Timestamp { get; internal set; }


        public object Event { get; internal set; }

        public UncommitedEvent(Id id, DateTime timestamp, object @event)
        {
            if (id == Id.None) throw new ArgumentOutOfRangeException("Id cannot be empty.", nameof(id));
            if (timestamp == default) throw new ArgumentOutOfRangeException(nameof(timestamp));
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            Id = id;
            Timestamp = timestamp;
            Event = @event;
        }
    }
}
