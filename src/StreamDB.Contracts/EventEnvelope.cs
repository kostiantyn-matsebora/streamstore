using System;
namespace StreamDB
{
    public sealed class EventEnvelope
    {
        public Id Id { get; internal set; }

        public DateTime Timestamp { get; internal set; }

        public object Event { get; internal set; }

        public EventEnvelope(Id id, DateTime timestamp, object @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            Id = id;
            Timestamp = timestamp;
            Event = @event;
        }
    }
}
