using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore
{

    internal class EventBatch<T> : IEnumerable<T> where T : IEventMetadata
    {
        readonly SortedList<int, T> items;

        public EventBatch(IEnumerable<T>? items = null)
        {
            this.items = new SortedList<int, T>();

            if (items != null && items.Any())
                AddRangeInternal(items);
        }

        public T[] Events => items.Values.ToArray();


        public int MinRevision => items.Any() ? items.First().Key : 0;

        public int MaxRevision => items.Any() ? items.Last().Key : 0;


        public IEnumerator<T> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            AddRangeInternal(items);
        }

        void AddRangeInternal(IEnumerable<T> items)
        {
            foreach (var e in items)
            {
                if (this.items.ContainsKey(e.Revision))
                    throw new InvalidOperationException(
                        "Collection of events contains event(s) with the same revision.");

                this.items.Add(e.Revision, e);
            }
        }
    }

    internal class EventMetadataBatch : EventBatch<IEventMetadata>
    {
        public EventMetadataBatch(IEnumerable<IEventMetadata>? items = null) : base(items)
        {
        }
    }
}
