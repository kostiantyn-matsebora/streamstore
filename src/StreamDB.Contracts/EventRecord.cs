using System;
using System.Collections.Generic;

namespace StreamDB
{
    public sealed class EventRecord: IEventMetadata
    {
        public Id Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Revision { get; set; }
        public string? Data { get; set; }
    }

    internal sealed class EventRecordBatch: EventBatch<EventRecord>
    {
        public EventRecordBatch(IEnumerable<EventRecord>? items = null) : base(items)
        {
        }
    }
}

