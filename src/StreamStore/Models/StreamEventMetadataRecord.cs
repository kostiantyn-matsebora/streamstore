using System;
using System.Collections.Generic;


namespace StreamStore
{
    internal class StreamEventMetadataRecord : IStreamEventMetadata
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }

        public IReadOnlyDictionary<string, string>? CustomProperties { get; set; }
    }
}
