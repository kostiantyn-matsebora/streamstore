using System;
using System.Collections.Generic;
using StreamStore.Models;
using StreamStore.Storage;
using StreamStore.Storage.Models;


namespace StreamStore
{
    class EventEnvelopeBuilder : IEventEnvelopeBuilder
    {
        Id id;
        DateTime timestamp = DateTime.Now;
        object @event = null!;
        EventCustomProperties customProperties = EventCustomProperties.Empty();

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

        public IEventEnvelopeBuilder WithCustomProperties(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            foreach (var kv in keyValuePairs)
            {
                customProperties.Add(kv.Key, kv.Value);
            }
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


        class EventEnvelope : IEventEnvelope
        {
            public Id Id { get; set; }
            public DateTime Timestamp { get; set; }
            public object Event { get; set; } = null!;

            public ICustomProperties CustomProperties { get; set; } = EventCustomProperties.Empty();
        }
    }
}
