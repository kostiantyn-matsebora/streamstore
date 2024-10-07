using System;
using System.Collections.Generic;


namespace StreamStore.S3.Models
{
    internal class S3Stream
    {
        S3Stream(S3StreamMetadata metadata, S3EventRecordCollection events)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            Events = events ?? throw new ArgumentNullException(nameof(metadata));
        }

        public S3StreamMetadata Metadata { get; }
        public S3EventRecordCollection Events { get; }

        public static S3Stream New(S3StreamMetadata metadata, S3EventRecordCollection events)
        {
            return new S3Stream(metadata, events);
        }
    }
}
