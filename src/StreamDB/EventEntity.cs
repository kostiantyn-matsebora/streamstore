using System;
namespace StreamDB
{
    public sealed class EventEntity: EventMetadata
    {
        public object Event { get; internal set; }

        public EventEntity(Id id, int revision, DateTime timestamp, object @event): base(id, revision, timestamp)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            Event = @event;
        }
    }
}
