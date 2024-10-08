using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace StreamStore.S3.Models
{
    internal sealed class S3StreamMetadata: S3EventMetadataCollection
    {
        public string? StreamId { get; }
        
        public int Revision => MaxRevision;

        internal S3StreamMetadata(Id streamId, IEnumerable<S3EventMetadata> eventMetadata): base(eventMetadata)
        {
            StreamId = streamId;
        }

        public static S3StreamMetadata New(Id streamId,IEnumerable<S3EventMetadata> eventMetadata)
        {
            return new S3StreamMetadata(streamId, eventMetadata);
        }

        public StreamMetadataRecord ToRecord()
        {
            return new StreamMetadataRecord(StreamId!, this.Select(m => m.ToRecord()));
        }
    }
}
