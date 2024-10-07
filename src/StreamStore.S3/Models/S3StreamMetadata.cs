using System.Collections.Generic;

namespace StreamStore.S3.Models
{
    internal sealed class S3StreamMetadata: S3EventMetadataCollection
    {
        public string? StreamId { get; set; }
        
        public int Revision => MaxRevision;


        internal S3StreamMetadata(Id streamId, IEnumerable<S3EventMetadata> eventMetadata): base(eventMetadata)
        {
            StreamId = streamId;
        }

        public static S3StreamMetadata New(Id streamId,IEnumerable<S3EventMetadata> eventMetadata)
        {
            return new S3StreamMetadata(streamId, eventMetadata);
        }


    }
}
