using System;

namespace StreamDB
{
    public sealed class EventEntity: IHasRevision, IHasId
    {
        public Id Id { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public int Revision { get; set; }
    }
}
