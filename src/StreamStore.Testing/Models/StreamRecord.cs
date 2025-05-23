using System;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore.Testing.Models
{
    public abstract class StreamRecord<T> where T : IStreamEventMetadata
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

    public sealed class StreamRecord : StreamRecord<IStreamEventRecord>
    {
        public StreamRecord(Id streamId) : this(streamId, Array.Empty<IStreamEventRecord>()) { }
        public StreamRecord(Id streamId, IEnumerable<IStreamEventRecord> records) : base(streamId, records) { }

        public bool IsEmpty => !Events.Any();
    }
}
