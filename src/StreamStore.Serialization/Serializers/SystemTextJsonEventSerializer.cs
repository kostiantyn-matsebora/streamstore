using System;


namespace StreamStore.Serialization
{
    public sealed class SystemTextJsonEventSerializer : StringEventSerializerBase
    {
        public SystemTextJsonEventSerializer(ITypeRegistry registry, bool compression) : base(registry, compression)
        {

        }

        public SystemTextJsonEventSerializer(ITypeRegistry registry, SerializationConfiguration configuration) : base(registry, configuration.CompressionEnabled)
        {

        }

        protected override string SerializeEventAsString(object value, Type type)
        {
            return System.Text.Json.JsonSerializer.Serialize(value);
        }

        protected override object DeserializeEventFromString(string value, Type type)
        {
            return System.Text.Json.JsonSerializer.Deserialize(value, type)!;
        }

        protected override string SerializeEnvelope(EventEnvelope envelope)
        {
            return System.Text.Json.JsonSerializer.Serialize(envelope);
        }

        protected override EventEnvelope DeserializeEnvelope(string value)
        {
            return System.Text.Json.JsonSerializer.Deserialize<EventEnvelope>(value)!;
        }
    }
}
