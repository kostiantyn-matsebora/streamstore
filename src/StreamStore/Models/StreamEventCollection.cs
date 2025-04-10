
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Models
{
    class StreamEventCollection : IEnumerable<IStreamEvent>
    {
        readonly List<IStreamEvent> events;

        public StreamEventCollection() : this(Enumerable.Empty<IStreamEvent>())
        {
        }
        public StreamEventCollection(IEnumerable<IStreamEvent> events)
        {
            this.events = events != null ? events.OrderBy(e => e.Revision).ToList() : throw new ArgumentNullException(nameof(events));
        }

        public IEnumerator<IStreamEvent> GetEnumerator()
        {
            return events.GetEnumerator();
        }

        public void Add(IStreamEvent @event)
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
