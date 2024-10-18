using System;
using ProtoBuf;
using System.IO;

namespace StreamStore.Serialization.Protobuf
{
    public class ProtobufEventSerializer : EventSerializerBase
    {
        public ProtobufEventSerializer(ITypeRegistry registry, bool compression = false): base(registry, compression) {  }

        protected override EventEnvelope DeserializeEnvelope(byte[] data)
        {
            using var stream = new MemoryStream(data);
            var envelope = Serializer.Deserialize<ProtobufEventEnvelope>(stream)!;
            return new EventEnvelope
            {
                Type = envelope.Type,
                Data = envelope.Data

            };
        }

        protected override object DeserializeEvent(byte[] data, Type type)
        {
            using var stream = new MemoryStream(data);
            return Serializer.Deserialize(type, stream)!;
        }

        protected override byte[] SerializeEnvelope(string type, byte[] eventData)
        {
           var envelope = new ProtobufEventEnvelope
           {
               Type = type,
               Data = eventData
           };

            using var stream = new MemoryStream();
            {
                Serializer.Serialize(stream, envelope!);
                return stream.ToArray();
            }
        }

        protected override byte[] SerializeEvent(object value, Type type)
        {
            using var stream = new MemoryStream();
            {
                Serializer.Serialize(stream, value);
                return stream.ToArray();
            }
        }


    }
}
