using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore
{
    public class RevisionedItemCollection<T>: IEnumerable<T> where T: IHasRevision
    {
        readonly SortedList<Revision, T> items = new SortedList<Revision, T>();

        public Revision MaxRevision => items.Any()? items.Last().Key : Revision.Zero;

        public Revision MinRevision => items.Any() ? items.First().Key : Revision.Zero;

        public RevisionedItemCollection(): this(Enumerable.Empty<T>())
        {
        }

        public RevisionedItemCollection(IEnumerable<T> records)
        {
           AddRange(records);
        }

        public void Clear()
        {
            items.Clear();
        }

        public void Add(T record)
        {
            items.Add(record.Revision, record);
        }

        public void AddRange(IEnumerable<T> records)
        {
            foreach (var record in records)
            {
                Add(record);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }
    }
}
