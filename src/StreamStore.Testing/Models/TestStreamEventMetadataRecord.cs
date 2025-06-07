using System;
using System.Collections.Generic;
using System.Linq;


namespace StreamStore.Testing.Models
{
    public class TestStreamEventMetadataRecord: IStreamEventMetadata
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }

        public IReadOnlyDictionary<string,string>? CustomProperties { get; set; }
    }

    public class TestStreamEventMetadataRecordCollection : RevisionedItemCollection<IStreamEventMetadata>
    {

        public DateTime? LastModified => this.Any() ? this.Last().Timestamp : null;

        public TestStreamEventMetadataRecordCollection() : base()
        {
        }

        public TestStreamEventMetadataRecordCollection(IEnumerable<IStreamEventMetadata> records) : base(records)
        {
        }
    }
}
