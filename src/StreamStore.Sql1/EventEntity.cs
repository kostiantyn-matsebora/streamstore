using System;

namespace StreamStore.SQL
{ 
    public class EventEntity
    {
        public string Id { get; set; }
        public string StreamId { get; set; }
        public int Revision { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[] Data { get; set; }
    }
}
