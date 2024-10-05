using System;
using System.Collections.Generic;


namespace StreamStore.S3.Models
{
    internal class S3Stream
    {
        public S3Stream(S3StreamMetadata metadata, IEnumerable<EventRecord> events)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            Events = events ?? throw new ArgumentNullException(nameof(metadata));
        }

        public S3StreamMetadata Metadata { get; }
        public IEnumerable<EventRecord> Events { get; }
    }
}
