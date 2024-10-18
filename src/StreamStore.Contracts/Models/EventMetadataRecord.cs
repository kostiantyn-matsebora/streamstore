using System;
using System.Collections.Generic;

namespace StreamStore
{
    public class EventMetadataRecord: IHasRevision
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }
    }

    public class EventMetadataRecordCollection : RevisionedItemCollection<EventMetadataRecord>
    {
        public EventMetadataRecordCollection() : base()
        {
        }

        public EventMetadataRecordCollection(IEnumerable<EventMetadataRecord> records) : base(records)
        {
        }
    }
}
