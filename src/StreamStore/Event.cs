using System;

namespace StreamStore
{
    public sealed class Event
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public object EventObject { get; set;  }
    }
}
