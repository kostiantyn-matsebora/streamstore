using System;
using System.IO;
using System.IO.Compression;

namespace StreamStore.Serialization
{
    public abstract class EventSerializerBase: IEventSerializer
    {
        readonly ITypeRegistry typeRegistry;
        readonly bool compression;

        protected EventSerializerBase(ITypeRegistry typeRegistry, bool compression = true)
        {
            this.typeRegistry = typeRegistry ?? throw new ArgumentNullException(nameof(typeRegistry));
            this.compression = compression;
        }

        public byte[] Serialize(object @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var typeName = GetNameByType(@event.GetType());

            var eventData = SerializeEvent(@event, @event.GetType());

            var envelopeData = SerializeEnvelope(typeName, eventData);

            if (compression)
            {
                envelopeData = Compress(envelopeData);
            }

            return envelopeData;
        }

        public object Deserialize(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var decompressed = compression ? Decompress(data) : data;

            var envelope = DeserializeEnvelope(decompressed);

            if (envelope == null || envelope.Data == null)
                throw new ArgumentException("Cannot deserialize event", nameof(data));

            var type = GetTypeByName(envelope.Type!);

            if (type == null)
                throw new ArgumentException($"Cannot find type {envelope.Type}", nameof(data));

            return DeserializeEvent(envelope!.Data, type)
                   ?? throw new InvalidOperationException("Deserialization returned null");
        }

        protected abstract byte[] SerializeEvent(object value, Type type);

        protected abstract EventEnvelope DeserializeEnvelope(byte[] data);

        protected abstract byte[] SerializeEnvelope(string type, byte[] eventData);

        protected abstract object DeserializeEvent(byte[] eventData, Type type);

        string GetNameByType(Type type)
        {
            return typeRegistry.ResolveNameByType(type);
        }

        Type GetTypeByName(string name)
        {
            return typeRegistry.ResolveTypeByName(name);
        }

        static byte[] Compress(byte[] serialized)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gzip.Write(serialized, 0, serialized.Length);
                }
                return ms.ToArray();
            }
        }

        static byte[] Decompress(byte[] compressed)
        {
            using (MemoryStream ms = new MemoryStream(compressed))
            using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
            using (MemoryStream outBuffer = new MemoryStream())
            {
                gzip.CopyTo(outBuffer);
                return outBuffer.ToArray();
            }
        }
    }
}



