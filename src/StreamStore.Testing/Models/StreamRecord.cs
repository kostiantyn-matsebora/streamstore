using System;
using System.Collections.Generic;
using System.Linq;
using StreamStore.Storage;

namespace StreamStore.Testing.Models
{
    public abstract class StreamRecord<T> where T : StreamEventMetadataRecord
    {
        public Revision Revision => Events.MaxRevision;

        public RevisionedItemCollection<T> Events { get; }

        public IEnumerable<Id> EventIds => Events.Select(x => x.Id);

        public Id Id { get; }

        protected StreamRecord(Id streamId, IEnumerable<T> records)
        {
            Id = streamId.ThrowIfHasNoValue(nameof(streamId));
            records.ThrowIfNull(nameof(records));

            Events = new RevisionedItemCollection<T>(records.ToArray());
        }
    }

    public sealed class StreamRecord : StreamRecord<StreamEventRecord>
    {
        public StreamRecord(Id streamId) : this(streamId, Array.Empty<StreamEventRecord>()) { }
        public StreamRecord(Id streamId, IEnumerable<StreamEventRecord> records) : base(streamId, records) { }

        public bool IsEmpty => !Events.Any();
    }
}
