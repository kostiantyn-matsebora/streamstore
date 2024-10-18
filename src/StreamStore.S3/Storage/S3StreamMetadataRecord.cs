using System.Linq;

namespace StreamStore.S3.Storage
{
    internal class S3StreamMetadataRecord // Entity is made for serialization
    {
        public S3StreamMetadataRecord() { }

        public Revision Revision => Events.Any() ? (Revision)Events.Max(x => x.Revision) : Revision.Zero;

        public S3StreamMetadataRecord(EventMetadataRecord[] events)
        {
            Events = events;
        }

        public EventMetadataRecord[]? Events { get; set; } = new EventMetadataRecord[0];
    }
}
