using System;
using System.Linq;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal class S3StreamMetadataRecord
    {
        public S3StreamMetadataRecord() { }
        internal S3StreamMetadataRecord(S3StreamMetadata metadata)
        {
            this.Events = metadata.Select(x => new S3EventMetadataRecord(x)).ToArray();
            this.StreamId = metadata.StreamId;
        }

        public S3EventMetadataRecord[]? Events { get; set; }
        public string? StreamId { get; set; }

        public S3StreamMetadata ToStreamMetadata()
        {
            return S3StreamMetadata.New(StreamId!, Events.Select(x => x.ToEventMetadata())); 
        }
    }


    internal class S3EventMetadataRecord
    {
        public S3EventMetadataRecord() { }
        internal S3EventMetadataRecord(S3EventMetadata metadata)
        {
            this.Id = metadata.Id;
            this.Revision = metadata.Revision;
        }

        public Id Id { get; set; }
        public int Revision { get; set; }

        internal S3EventMetadata ToEventMetadata()
        {
           return new S3EventMetadata(Id, Revision);
        }
    }
}
