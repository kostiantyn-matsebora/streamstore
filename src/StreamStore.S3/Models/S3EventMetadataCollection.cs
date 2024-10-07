using System.Collections.Generic;


namespace StreamStore.S3.Models
{
    internal class S3EventMetadataCollection : RevisionedItemCollection<S3EventMetadata>
    {     
        public S3EventMetadataCollection(IEnumerable<S3EventMetadata> records) : base(records)
        {
        }
    }
}