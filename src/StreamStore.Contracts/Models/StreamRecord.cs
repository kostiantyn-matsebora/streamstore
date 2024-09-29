using System;
using System.Collections.Generic;
using System.Linq;
namespace StreamStore
{
    public abstract class StreamRecord<T> where T : EventMetadataRecord
    {

        public Id Id { get; }

        public int Revision { get; }

        public T[] Events { get; }

        public StreamRecord(Id id, IEnumerable<T> records)
        {
            if (id == Id.None)
                throw new ArgumentOutOfRangeException("Id cannot be empty.", nameof(id));
            Id = id;

            if (records == null)
                throw new ArgumentNullException(nameof(records));
            
            Events = records.ToArray();

            Revision = Events.Any() ? Events.Max(e => e.Revision) : 0;
        }
    }

    public sealed class StreamRecord : StreamRecord<EventRecord>
    {
        public StreamRecord(Id id): this(id, new EventRecord[0]) { }
        public StreamRecord(Id id, IEnumerable<EventRecord> records) : base(id, records) { }
    }

    public sealed class StreamMetadataRecord : StreamRecord<EventMetadataRecord>
    {
        public StreamMetadataRecord(Id id, IEnumerable<EventMetadataRecord> records) : base(id, records)
        {
        }
    }
}
