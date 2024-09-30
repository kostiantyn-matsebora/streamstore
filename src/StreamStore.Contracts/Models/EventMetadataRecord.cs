using System;
namespace StreamStore
{
    public class EventMetadataRecord
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }
    }
}
