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
            this.Id = streamId.ThrowIfHasNoValue(nameof(streamId));
            records.ThrowIfNull(nameof(records));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            Events = new RevisionedItemCollection<T>(records.ToArray());
        }
    }

    public sealed class StreamRecord : StreamRecord<EventRecord>
    {
        public StreamRecord(Id streamId) : this(streamId, new EventRecord[0]) { }
        public StreamRecord(Id streamId, IEnumerable<EventRecord> records) : base(streamId, records) { }

        public bool IsEmpty => !Events.Any();
    }
}
