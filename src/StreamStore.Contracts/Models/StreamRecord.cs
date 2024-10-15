using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore
{
    public abstract class StreamRecord<T>  where T : EventMetadataRecord
    {
        public Id Id { get; set; }
        public Revision Revision => Events.MaxRevision;

        public RevisionedItemCollection<T> Events { get; }

        public IEnumerable<Id> EventIds => Events.Select(x => x.Id);

        protected StreamRecord(Id id, IEnumerable<T> records)
        {
            id.ThrowIfHasNoValue();

            Id = id;

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            Events = new RevisionedItemCollection<T>(records.ToArray());
        }
    }

    public sealed class StreamRecord : StreamRecord<EventRecord>
    {
        public StreamRecord(Id id) : this(id, new EventRecord[0]) { }
        public StreamRecord(Id id, IEnumerable<EventRecord> records) : base(id, records) { }

        public bool IsEmpty => Events.Count() == 0;
    }

    public sealed class StreamMetadataRecord : StreamRecord<EventMetadataRecord>
    {
        public StreamMetadataRecord(Id id, IEnumerable<EventMetadataRecord> records) : base(id, records)
        {
        }
    }
}
