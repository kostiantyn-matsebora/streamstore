using System;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore.S3.Models
{
    internal class S3EventRecordCollection : RevisionedItemCollection<S3EventRecord>
    {
        public S3EventRecordCollection() : base(Enumerable.Empty<S3EventRecord>())
        {
        }

        public S3EventRecordCollection(IEnumerable<S3EventRecord> records) : base(records)
        {
        }

        public S3EventMetadataCollection ToEventMetadata()
        {
            return new S3EventMetadataCollection(this.Select(record => new S3EventMetadata(record.Id, record.Revision)));
        }
    }

    internal class S3EventRecord : EventRecord, IEquatable<EventRecord>
    {
        public bool Equals(EventRecord other)
        {
            if (other == null) return false;
            return Equals(other.Id);
        }


        public static implicit operator S3EventMetadata(S3EventRecord record) => new S3EventMetadata(record.Id, record.Revision);
    }
}
