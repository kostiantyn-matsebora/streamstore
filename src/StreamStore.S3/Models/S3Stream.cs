using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamStore.S3.Models
{
    internal class S3Stream
    {
        S3Stream(S3StreamMetadata metadata, IEnumerable<EventRecord> events)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            Events = events != null ? events.ToArray() : throw new ArgumentNullException(nameof(metadata));
        }

        public Id Id => Metadata.StreamId!;

        public S3StreamMetadata Metadata { get; }
        public EventRecord[] Events { get; }

        public static S3Stream New(S3StreamMetadata metadata, IEnumerable<EventRecord> events)
        {
            return new S3Stream(metadata, events);
        }
    }
}
