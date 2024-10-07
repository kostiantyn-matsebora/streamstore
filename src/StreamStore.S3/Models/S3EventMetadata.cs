using System;

namespace StreamStore.S3.Models
{
    internal class S3EventMetadata: IHasRevision, IEquatable<S3EventMetadata>, IEquatable<Id>
    {

        public Id Id { get; }

        public int Revision { get; }

        public bool Equals(S3EventMetadata other)
        {
            if (other == null) return false;
            return Equals(other.Id);
        }

        public bool Equals(Id other)
        {
            return Id.Equals(other);
        }

        public S3EventMetadata(Id id, int revision)
        {
            Id = id;
            Revision = revision;
        }
    }
}
