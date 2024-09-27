using System;
using System.Collections.Generic;
namespace StreamDB
{
    public abstract class StreamRecord<T> where T: EventMetadataRecord
    {
        readonly EventBatch<T> batch;

        public Id Id { get; }

        public int Revision => batch.MaxRevision;

        public T[] Events => batch.Events;

        public StreamRecord(Id id, IEnumerable<T> records)
        {
            Id = id;
            if (records == null)
                throw new ArgumentNullException(nameof(records));

            batch = new EventBatch<T>(records);
        }
    }

    public sealed class StreamRecord: StreamRecord<EventRecord>
    {
        public StreamRecord(Id id, IEnumerable<EventRecord> records) : base(id, records)
        {
        }
    }

    public sealed class StreamMetadataRecord: StreamRecord<EventMetadataRecord>
    {
        public StreamMetadataRecord(Id id, IEnumerable<EventMetadataRecord> records) : base(id, records)
        {
        }
    }

}
