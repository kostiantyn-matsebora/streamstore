using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{

    internal class S3EventMetadataRecord // Entity is made for serialization
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
