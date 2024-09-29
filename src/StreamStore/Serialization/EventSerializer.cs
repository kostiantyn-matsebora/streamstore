// generate tests for this file

using System;

namespace StreamStore.Serialization
{
    internal class EventSerializer : IEventSerializer
    {
        public string Serialize(object @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var envelope = new EventEnvelope
            {
                Type = TypeRegistry.Instance.ByType(@event.GetType()),
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(@event)
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(envelope);
        }

        public object Deserialize(string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<EventEnvelope>(data);

            if (envelope == null || envelope.Data == null)
                throw new ArgumentException("Cannot deserialize event", nameof(data));

            var type = TypeRegistry.Instance.ByName(envelope.Type!);

            if (type == null)
                throw new ArgumentException($"Cannot find type {envelope.Type}", nameof(data));

            return Newtonsoft.Json.JsonConvert.DeserializeObject(envelope.Data, type)
                   ?? throw new InvalidOperationException("Deserialization returned null");
        }

        class EventEnvelope
        {
            public string? Type { get; set; }
            public string? Data { get; set; }
        }
    }
}
