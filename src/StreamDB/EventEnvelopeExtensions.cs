using Newtonsoft.Json;

namespace StreamDB
{
    public static class EventEnvelopeExtensions
    {
        public static EventData ToEventData(this EventEnvelope e, int revision) {
            return new EventData
            {

                Id = e.Id,
                Timestamp = e.Timestamp,
                Data = JsonConvert.SerializeObject(e.Event)
            }; 
        }
    }
}
