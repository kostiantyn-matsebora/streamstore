
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Models
{
    class StreamEventCollection : IEnumerable<IStreamEventEnvelope>
    {
        readonly List<IStreamEventEnvelope> events;

        public StreamEventCollection() : this(Enumerable.Empty<IStreamEventEnvelope>())
        {
        }
        public StreamEventCollection(IEnumerable<IStreamEventEnvelope> events)
        {
            this.events = events != null ? events.OrderBy(e => e.Revision).ToList() : throw new ArgumentNullException(nameof(events));
        }

        public IEnumerator<IStreamEventEnvelope> GetEnumerator()
        {
            return events.GetEnumerator();
        }

        public void Add(IStreamEventEnvelope @event)
        {
            @event.ThrowIfNull(nameof(@event));
            events.Add(@event);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return events.GetEnumerator();
        }
    }
}
