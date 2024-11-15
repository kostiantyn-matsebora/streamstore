namespace StreamStore.Testing.Models
{
    public abstract class StreamRecord<T> where T : EventMetadataRecord
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

    public sealed class StreamRecord : StreamRecord<EventRecord>
    {
        public StreamRecord(Id streamId) : this(streamId, Array.Empty<EventRecord>()) { }
        public StreamRecord(Id streamId, IEnumerable<EventRecord> records) : base(streamId, records) { }

        public bool IsEmpty => !Events.Any();
    }
}
