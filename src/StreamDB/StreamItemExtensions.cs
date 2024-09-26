using System;
using System.Linq;
using Newtonsoft.Json;

namespace StreamDB
{
    internal static class StreamItemExtensions
    {
        public static EventRecord ToEventRecord(this IStreamItem e, int revision, Func<object, string> serializer)
        {
            return new EventRecord
            {
                Id = e.Id,
                Timestamp = e.Timestamp,
                Revision = revision,
                Data = serializer(e.Event) // Ensure Data is not null
            };
        }

        public static EventRecord[] ToEventRecordArray(this IStreamItem[] items, int revision, Func<object, string> serializer)
        {
            return items.Select(i => i.ToEventRecord(revision++, serializer)).ToArray();
        }
    }
}
