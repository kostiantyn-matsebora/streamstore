using System;
using Polenter.Serialization;
using Polenter.Serialization.Advanced.Serializing;
using Serializer = Polenter.Serialization.SharpSerializer;

namespace StreamStore.Serialization.SharpSerializer
{
    public class SharpEventSerializer : IEventSerializer
    {
        readonly Serializer serializer;
        public SharpEventSerializer(bool binary = true)
        {
            serializer = new Serializer(new SharpSerializerBinarySettings { 
                Mode = BinarySerializationMode.SizeOptimized,
                IncludeAssemblyVersionInTypeName = false,
                IncludeCultureInTypeName = false,
                IncludePublicKeyTokenInTypeName = false,
            });
        }

        public object Deserialize(byte[] data)
        {
            using var stream = new System.IO.MemoryStream(data);
            return serializer.Deserialize(stream);

        }

        public byte[] Serialize(object @event)
        {
            using var stream = new System.IO.MemoryStream();
            serializer.Serialize(@event, stream);
            return stream.ToArray();
        }
    }

    class TypeNameConverter : ITypeNameConverter
    {
      
        public TypeNameConverter()
        {
            
        }

        public Type? ConvertToType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;
            return TypeRegistry.Instance.ByName(typeName);
        }

        public string? ConvertToTypeName(Type type)
        {
            if (type == null) return null;

            return TypeRegistry.Instance.ByType(type);
        }
    }
}
