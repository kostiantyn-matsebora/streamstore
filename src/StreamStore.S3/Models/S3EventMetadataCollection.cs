using System.Collections.Generic;
using System.Linq;

namespace StreamStore.S3.Models
{
    internal class S3EventMetadataCollection : RevisionedItemCollection<S3EventMetadata>
    {
        // For serialization only
        public IEnumerable<S3EventMetadata> Events
        {
            get
            {
                return this;
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
}