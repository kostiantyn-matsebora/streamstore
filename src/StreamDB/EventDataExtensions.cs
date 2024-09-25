using Newtonsoft.Json;

namespace StreamDB
{
    internal static class EventDataExtensions
    {

        public static EventEnvelope ToEvent(this EventData data)
        {
            return new EventEnvelope(data.Id, data.Timestamp, JsonConvert.DeserializeObject(data.Data));
        }
    }
}
