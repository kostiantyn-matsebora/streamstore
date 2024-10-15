using System.Collections.Generic;


namespace StreamStore
{
    public static class StreamRecordExtension
    {
        public static StreamRecord AddRange(this StreamRecord record, IEnumerable<EventRecord> events)
        {
            record.Events.AddRange(events);
            return record;
        }
    }
}
