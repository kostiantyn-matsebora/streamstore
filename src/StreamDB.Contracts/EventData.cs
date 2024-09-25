using System;

namespace StreamDB
{
    public sealed class EventData
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public int Revision { get; set; }
    }
}
