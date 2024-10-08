using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore
{
    public class RevisionedItemCollection<T>: IEnumerable<T> where T: IHasRevision
    {
        readonly SortedList<int, T> items = new SortedList<int, T>();

        public int MaxRevision => items.Any()? items.Last().Key : 0;

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
