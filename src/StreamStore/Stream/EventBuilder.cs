using System;

namespace StreamStore
{
    internal class EventBuilder : IEventBuilder
    {
        Id id;
        DateTime timestamp = DateTime.Now;
        object @event = null!;

        public IEventBuilder WithId(Id id)
        {
            this.id = id;
            return this;
        }

        public IEventBuilder Dated(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        public IEventBuilder WithEvent(object @event)
        {
            this.@event = @event;
            return this;
        }

        internal Event Build()
        {
            id.ThrowIfHasNoValue(nameof(id));
            timestamp.ThrowIfMinValue(nameof(timestamp));
            @event.ThrowIfNull(nameof(@event));
            return new Event
            {
                Id = id,
                Timestamp = timestamp,
                EventObject = @event
            };
        }
    }
}
