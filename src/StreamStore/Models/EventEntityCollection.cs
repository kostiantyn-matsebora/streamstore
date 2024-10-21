
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Models
{
    public class EventEntityCollection : IEnumerable<EventEntity>
    {
        readonly EventEntity[] events;
        public EventEntityCollection(IEnumerable<EventEntity> events)
        {
            this.events = events != null ? events.OrderBy(e => e.Revision).ToArray() : throw new ArgumentNullException(nameof(events));
        }

        public Revision MaxRevision => events.Max(x => x.Revision);

        public IEnumerator<EventEntity> GetEnumerator()
        {
            return (IEnumerator<EventEntity>)events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return events.GetEnumerator();
        }
    }
}
