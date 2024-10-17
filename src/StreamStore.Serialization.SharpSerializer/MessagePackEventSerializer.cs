using System;
using MessagePack;

namespace StreamStore.Serialization.SharpSerializer
{
    public class MessagePackEventSerializer : EventSerializerBase
    {
        protected override object DeserializeObject(byte[] data, Type type)
        {
           return MessagePackSerializer.Deserialize(type, data)!;
        }

        protected override byte[] SerializeObject(object @event, Type type)
        {
            return MessagePackSerializer.Serialize(type, @event);
        }
    }
}
