using System;
using System.Collections.Generic;

namespace StreamStore.Storage
{
    public class StreamEventMetadataRecord: IStreamEventMetadata
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }
    }

    public class StreamEventMetadataRecordCollection : RevisionedItemCollection<IStreamEventMetadata>
    {
        public StreamEventMetadataRecordCollection() : base()
        {
        }

        public StreamEventMetadataRecordCollection(IEnumerable<IStreamEventMetadata> records) : base(records)
        {
        }
    }
}
