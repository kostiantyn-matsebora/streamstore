using System;

namespace StreamStore.Serialization
{
    public abstract class StringEventSerializerBase : EventSerializerBase
    {
        protected StringEventSerializerBase(ITypeRegistry registry, bool compression = false): base(registry, compression)
        {
        }

        protected abstract string SerializeEventAsString(object value, Type type);

        protected abstract object DeserializeEventFromString(string value, Type type);


        protected abstract string SerializeEnvelope(EventEnvelope envelope);

        protected abstract EventEnvelope DeserializeEnvelope(string value);

        protected override byte[] SerializeEnvelope(string type, byte[] eventData)
        {
            var envelope = new EventEnvelope
            {
                Type = type,
                Data = eventData
            };

            return System.Text.Encoding.UTF8.GetBytes(SerializeEnvelope(envelope));
        }

        protected override byte[] SerializeEvent(object value, Type type)
        {
            return System.Text.Encoding.UTF8.GetBytes(SerializeEventAsString(value, type));
        }
 
        protected override EventEnvelope DeserializeEnvelope(byte[] data)
        {
            return DeserializeEnvelope(System.Text.Encoding.UTF8.GetString(data));
        }

        protected override object DeserializeEvent(byte[] eventData, Type type)
        {
            return DeserializeEventFromString(System.Text.Encoding.UTF8.GetString(eventData), type);
        }
    }
}



