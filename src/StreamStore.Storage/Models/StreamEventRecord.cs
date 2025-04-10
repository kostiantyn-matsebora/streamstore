using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Storage
{
    public class StreamEventRecord : StreamEventMetadataRecord, IStreamEventRecord, IEventRecord
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public byte[] Data { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    public class StreamEventRecordCollection : RevisionedItemCollection<IStreamEventRecord>
    {
        public StreamEventRecordCollection(IEnumerable<IStreamEventRecord> records) : base(records)
        {
        }

        public StreamEventRecordCollection() : base(Enumerable.Empty<IStreamEventRecord>())
        {
        }
    }
}

