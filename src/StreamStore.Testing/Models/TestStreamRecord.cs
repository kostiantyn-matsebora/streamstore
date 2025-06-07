using System;
using System.Collections.Generic;
using System.Linq;
using StreamStore.Extensions;


namespace StreamStore.Testing.Models
{
    public abstract class TestStreamRecord<T> where T : IStreamEventMetadata
    {
        public Revision Revision => Events.MaxRevision;

        public RevisionedItemCollection<T> Events { get; }

        public IEnumerable<Id> EventIds => Events.Select(x => x.Id);

        public Id Id { get; }

        protected TestStreamRecord(Id streamId, IEnumerable<T> records)
        {
            Id = streamId.ThrowIfHasNoValue(nameof(streamId));
            records.ThrowIfNull(nameof(records));

            Events = [.. records.ToArray()];
        }
    }

    public sealed class TestStreamRecord : TestStreamRecord<IStreamEventRecord>
    {
        public TestStreamRecord(Id streamId) : this(streamId, Array.Empty<IStreamEventRecord>()) { }
        public TestStreamRecord(Id streamId, IEnumerable<IStreamEventRecord> records) : base(streamId, records) { }

        public bool IsEmpty => !Events.Any();
    }
}
