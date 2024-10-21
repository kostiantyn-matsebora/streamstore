using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore
{
    public abstract class StreamRecord<T>  where T : EventMetadataRecord
    {
        public Revision Revision => Events.MaxRevision;

        public RevisionedItemCollection<T> Events { get; }

        public IEnumerable<Id> EventIds => Events.Select(x => x.Id);

        protected StreamRecord(IEnumerable<T> records)
        {
            if (records == null)
                throw new ArgumentNullException(nameof(records));

            Events = new RevisionedItemCollection<T>(records.ToArray());
        }
    }

    public sealed class StreamRecord : StreamRecord<EventRecord>
    {
        public StreamRecord() : this(new EventRecord[0]) { }
        public StreamRecord(IEnumerable<EventRecord> records) : base(records) { }

        public bool IsEmpty => !Events.Any();
    }

    public sealed class StreamMetadataRecord : StreamRecord<EventMetadataRecord>
    {
        public StreamMetadataRecord(): this(new EventMetadataRecord[0])
        {

        }

        public StreamMetadataRecord(IEnumerable<EventMetadataRecord> records) : base(records)
        {
        }
    }
}
