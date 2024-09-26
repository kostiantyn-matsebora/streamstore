using System;
using System.Collections.Generic;
namespace StreamDB
{
    public sealed class StreamRecord
    {
        readonly EventBatch<EventRecord> batch;

        public Id Id { get; }

        public int Revision => batch.MaxRevision;

        public EventRecord[] Events => batch.Events;

        public StreamRecord(Id id, IEnumerable<EventRecord> records)
        {
            Id = id;
            if (records == null)
                throw new ArgumentNullException(nameof(records));

            batch = new EventBatch<EventRecord>(records);
        }
    }
}
