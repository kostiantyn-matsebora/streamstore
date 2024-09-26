using System;
using System.Linq;


namespace StreamDB
{
    internal static class EventRecordExtensions
    {
        public static IStreamItem ToStreamItem(this EventRecord data, Func<string?, object> deserializer)
        {
            return new StreamItem(
                data.Id, 
                data.Revision, 
                data.Timestamp,
                deserializer(data.Data));
        }

        public static IStreamItem[] ToStreamItemArray(this EventRecord[] records, Func<string?, object> deserializer)
        {
            return records.Select(r => r.ToStreamItem(deserializer)).ToArray();
        }
    }
}
