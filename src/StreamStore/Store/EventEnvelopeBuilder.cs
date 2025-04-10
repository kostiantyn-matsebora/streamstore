using System;
using StreamStore.Models;

namespace StreamStore
{
    class EventEnvelopeBuilder : IEventEnvelopeBuilder
    {
        Id id;
        DateTime timestamp = DateTime.Now;
        object @event = null!;

        public IEventEnvelopeBuilder WithId(Id id)
        {
            this.id = id;
            return this;
        }

        public IEventEnvelopeBuilder Dated(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        public IEventEnvelopeBuilder WithEvent(object @event)
        {
            this.@event = @event;
            return this;
        }

        internal IEventEnvelope Build()
        {
            id.ThrowIfHasNoValue(nameof(id));
            timestamp.ThrowIfMinValue(nameof(timestamp));
            @event.ThrowIfNull(nameof(@event));
            return new EventEnvelope
            {
                Id = id,
                Timestamp = timestamp,
                Event = @event
            };
        }
    }
}
