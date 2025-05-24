using System;

namespace StreamStore.Sql
{ 
    public class EventMetadataEntity
    {
        public string? Id { get; set; }
        public string? StreamId { get; set; }
        public int Revision { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
