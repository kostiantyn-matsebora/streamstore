using System.Linq;

namespace StreamStore.S3.Storage
{
    internal class S3StreamMetadataRecord // Entity is made for serialization
    {
        public S3StreamMetadataRecord() { }

        public S3StreamMetadataRecord(EventMetadataRecord[] events)
        {
            Events = events;
        }


        public Revision Revision => Events.Any() ? (Revision)Events.Max(x => x.Revision) : Revision.Zero;

        public EventMetadataRecord[]? Events { get; set; } = new EventMetadataRecord[0];
    }
}
