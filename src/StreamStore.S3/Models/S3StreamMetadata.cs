using System.Collections.Generic;
using System.Linq;
using Amazon.S3.Model;

namespace StreamStore.S3.Models
{
    internal sealed class S3StreamMetadata: S3EventMetadataCollection
    {


        public string? StreamId { get; set; }
        
        public int Revision => MaxRevision;


        S3StreamMetadata(Id streamId, IEnumerable<S3EventMetadata> eventMetadata)
        {
            StreamId = streamId;
            AddRange(eventMetadata);
        }

        public static S3StreamMetadata New(Id streamId,IEnumerable<S3EventMetadata> eventMetadata)
        {
            return new S3StreamMetadata(streamId, eventMetadata);
        }
    }
}
