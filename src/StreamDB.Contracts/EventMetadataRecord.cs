using System;


namespace StreamDB
{
    public class EventMetadataRecord: IEventMetadata
    {
        public Id Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int Revision { get; set; }
    }
}
