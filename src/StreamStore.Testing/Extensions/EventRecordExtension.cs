using System.Collections.Generic;
using System.Linq;
using StreamStore.Storage;

namespace StreamStore.Testing
{
    internal static class EventRecordExtension
    {

        public static TestEventEnvelope[] ToEventEnvelopes(this IEnumerable<IStreamEventRecord> records)
        {
           return records.Select(e => new TestEventEnvelope { Id = e.Id, Event = new { }, Timestamp = e.Timestamp }).ToArray();
        }
    }
}
