using System;
using Newtonsoft.Json;

namespace StreamDB
{
    internal static class EventEnvelopeExtensions
    {
        public static EventEntity ToEventEntity(this EventEnvelope e, int revision, Func<object, string> serializer) {
            return new EventEntity
            {
                Id = e.Id,
                Timestamp = e.Timestamp,
                Revision = revision,
                Data = serializer(e.Event)
            }; 
        }
    }
}
