using System;


namespace StreamDB
{
    internal static class EventEntityExtensions
    {
        public static EventEnvelope ToEvent(this EventEntity data, Func<string, object> deserializer)
        {
            return new EventEnvelope(
                data.Id, 
                data.Revision, 
                data.Timestamp, 
                deserializer(data.Data));
        }
    }
}
