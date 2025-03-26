using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Testing
{
    internal static class EventRecordExtension
    {

        public static Event[] ToEvents(this IEnumerable<EventRecord> records)
        {
           return records.Select(e => new Event { Id = e.Id, EventObject = new { }, Timestamp = e.Timestamp }).ToArray();
        }
    }
}
