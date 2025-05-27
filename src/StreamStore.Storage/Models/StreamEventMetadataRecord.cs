using System;
using System.Collections.Generic;
using System.Linq;
using StreamStore.Models;


namespace StreamStore.Storage
{
    public class StreamEventMetadataRecord: IStreamEventMetadata
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }

        public IReadOnlyDictionary<string,string>? CustomProperties { get; set; }
    }

    public class StreamEventMetadataRecordCollection : RevisionedItemCollection<IStreamEventMetadata>
    {

        public DateTime? LastModified => this.Any() ? this.Last().Timestamp : (DateTime?)null;

        public StreamEventMetadataRecordCollection() : base()
        {
        }

        public StreamEventMetadataRecordCollection(IEnumerable<IStreamEventMetadata> records) : base(records)
        {
        }
    }
}
