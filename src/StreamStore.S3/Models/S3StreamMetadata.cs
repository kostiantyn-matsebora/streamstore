using System.Collections.Generic;
using System.Linq;

namespace StreamStore.S3.Models
{
    internal sealed class S3StreamMetadata
    {
        public string? StreamId { get; set; }
        public int Revision { get; set; }

        public string[]? EventIds { get; set; }

        public static S3StreamMetadata New(Id streamId, int revision, IEnumerable<string> eventIds)
        {
            return new S3StreamMetadata
            {
                StreamId = streamId,
                Revision = revision,
                EventIds = eventIds.ToArray()
            };
        }
    }
}
