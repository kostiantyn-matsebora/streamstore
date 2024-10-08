using System.Linq;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal class S3StreamMetadataRecord // Entity is made for serialization
    {
        public S3StreamMetadataRecord() { }
        internal S3StreamMetadataRecord(S3StreamMetadata metadata)
        {
            Events = metadata.Select(x => x.ToRecord()).ToArray();
            StreamId = metadata.StreamId;
        }

        public EventMetadataRecord[]? Events { get; set; }
        public string? StreamId { get; set; }


        public S3StreamMetadata ToMetadata()
        {
            return S3StreamMetadata.New(StreamId!, Events.Select(x => x.ToMetadata())); 
        }
    }
}
