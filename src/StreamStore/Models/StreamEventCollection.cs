﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Models
{
    public class StreamEventCollection : IEnumerable<StreamEvent>
    {
        readonly List<StreamEvent> events;

        public StreamEventCollection() : this(Enumerable.Empty<StreamEvent>())
        {
        }
        public StreamEventCollection(IEnumerable<StreamEvent> events)
        {
            this.events = events != null ? events.OrderBy(e => e.Revision).ToList() : throw new ArgumentNullException(nameof(events));
        }

        public Revision MaxRevision => events.Max(x => x.Revision);

        public IEnumerator<StreamEvent> GetEnumerator()
        {
            return events.GetEnumerator();
        }

        public void Add(StreamEvent @event)
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
