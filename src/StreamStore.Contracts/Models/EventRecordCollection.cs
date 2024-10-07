using System.Collections.Generic;


namespace StreamStore
{
    public class EventRecordCollection: RevisionedItemCollection<EventRecord>
    {
        public EventRecordCollection(IEnumerable<EventRecord> records) : base(records)
        {
        }
    }
}
