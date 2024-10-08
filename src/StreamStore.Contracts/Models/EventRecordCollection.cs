using System.Collections.Generic;
using System.Linq;


namespace StreamStore
{
    public class EventRecordCollection: RevisionedItemCollection<EventRecord>
    {
        public EventRecordCollection(IEnumerable<EventRecord> records) : base(records)
        {
        }

        public EventRecordCollection(): base(Enumerable.Empty<EventRecord>())
        {
        }
    }
}
