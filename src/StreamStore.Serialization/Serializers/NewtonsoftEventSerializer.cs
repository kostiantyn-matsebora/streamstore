using System;
using Newtonsoft.Json.Linq;

namespace StreamStore.Serialization
{
    public sealed class NewtonsoftEventSerializer : StringEventSerializerBase
    {
        public NewtonsoftEventSerializer(ITypeRegistry registry, StreamStoreConfiguration configuration) : base(registry, configuration.CompressionEnabled)
        {
        }

        public NewtonsoftEventSerializer(ITypeRegistry registry, bool compression) : base(registry, compression)
        {
        }


        protected override string SerializeEventAsString(object value, Type type)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        protected override object DeserializeEventFromString(string value, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, type)!;
        }

        protected override string SerializeEnvelope(EventEnvelope envelope)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(envelope);
        }

        protected override EventEnvelope DeserializeEnvelope(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EventEnvelope>(value)!;
        }
    }
}
