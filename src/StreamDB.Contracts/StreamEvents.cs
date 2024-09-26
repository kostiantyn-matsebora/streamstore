using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace StreamDB
{
    internal class StreamEvents<T> : IEnumerable<T> where T : IHasRevision
    {
        readonly SortedList<int, T> events;

        public StreamEvents(IEnumerable<T> events) 
        {

            this.events = new SortedList<int, T>();

            AddRangeInternal(events);
        }

        public T[] Events => events.Values.ToArray();


        public int? MinRevision => events.FirstOrDefault().Key;

        public int? MaxRevision => events.LastOrDefault().Key;

        public IEnumerator<T> GetEnumerator()
        {
            return events.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return events.Values.GetEnumerator();
        }

        public void AddRange(IEnumerable<T> events)
        {
            AddRangeInternal(events);
        }

        void AddRangeInternal(IEnumerable<T> events)
        {
            foreach (var e in events)
            {
                if (this.events.ContainsKey(e.Revision))
                    throw new InvalidOperationException("Collection of events data contains events with the same revision.");
                this.events.Add(e.Revision, e);
            }
        }
    }
}
