using System;
using StreamStore.S3.Operations;

namespace StreamStore.S3.Models
{
    internal class S3EventMetadata: IHasRevision, IEquatable<S3EventMetadata>, IEquatable<Id>
    {
        public Id Id { get; }

        public int Revision { get; }

        public DateTime Timestamp { get; }

        public S3EventMetadata(EventMetadataRecord record)
        {
            Id = record.Id;
            Revision = record.Revision;
            Timestamp = record.Timestamp;
        }

        public S3EventMetadata(Id id, int revision)
        {
            Id = id;
            Revision = revision;
        }

        public bool Equals(S3EventMetadata other)
        {
            if (other == null) return false;
            return Equals(other.Id);
        }

        public bool Equals(Id other)
        {
            return Id.Equals(other);
        }

        public EventMetadataRecord ToRecord()
        {
            return new EventMetadataRecord
            {
                Id = Id,
                Timestamp = Timestamp,
                Revision = Revision
            };
        }
    }
}
