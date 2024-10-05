using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamStore;
using StreamStore.S3.Models;

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

    internal class S3EventMetadataCollection: RevisionedItemCollection<S3EventMetadata>
    {
        
        // For serialization only
        public S3EventMetadata[] Events
        {
            get
            {
               return this.ToArray();
            }
            set
            {
                Clear();
                AddRange(value);
            }
        }

        // For serialization only
        public S3EventMetadataCollection() : base(Enumerable.Empty<S3EventMetadata>())
        {
        }

        public S3EventMetadataCollection(IEnumerable<S3EventMetadata> records) : base(records)
        {
        }
}
