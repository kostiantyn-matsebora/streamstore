
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Models
{
    public class StreamEventCollection : IEnumerable<StreamEvent>
    {
        readonly StreamEvent[] events;
        public StreamEventCollection(IEnumerable<StreamEvent> events)
        {
            this.events = events != null ? events.OrderBy(e => e.Revision).ToArray() : throw new ArgumentNullException(nameof(events));
        }

        public Revision MaxRevision => events.Max(x => x.Revision);

        public IEnumerator<StreamEvent> GetEnumerator()
        {
            return (IEnumerator<StreamEvent>)events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return events.GetEnumerator();
        }
    }
}
