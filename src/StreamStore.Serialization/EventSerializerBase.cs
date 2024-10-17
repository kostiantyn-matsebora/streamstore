
using System;
using System.IO;
using System.IO.Compression;

namespace StreamStore.Serialization
{
    public abstract class EventSerializerBase<TEnvelope> : IEventSerializer where TEnvelope : class, IEventEnvelope
    {
        readonly bool compression;

        protected EventSerializerBase(bool compression = true)
        {
            this.compression = compression;
        }

        public byte[] Serialize(object @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var typeName = GetNameByType(@event.GetType());

            var envelope = CreateEnvelope(SerializeObject(@event, @event.GetType()), typeName);

            var serialized = SerializeObject(envelope, envelope.GetType());
            if (!compression) return serialized;

            return Compress(serialized);
        }

        public object Deserialize(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (compression) data = Decompress(data);

            var envelope = (TEnvelope?)DeserializeObject(data, typeof(TEnvelope));

            if (envelope == null || envelope.Data == null)
                throw new ArgumentException("Cannot deserialize event", nameof(data));

            var type = GetTypeByName(envelope.Type!);

            if (type == null)
                throw new ArgumentException($"Cannot find type {envelope.Type}", nameof(data));

            return DeserializeObject(envelope!.Data, type)
                   ?? throw new InvalidOperationException("Deserialization returned null");
        }

        protected abstract byte[] SerializeObject(object @event, Type type);
        protected abstract object DeserializeObject(byte[] data, Type type);
        protected abstract TEnvelope CreateEnvelope(byte[] data, string typeName);

        string GetNameByType(Type type)
        {
            return TypeRegistry.Instance.ByType(type);
        }

        Type GetTypeByName(string name)
        {
            return TypeRegistry.Instance.ByName(name);
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

    public abstract class EventSerializerBase : EventSerializerBase<EventEnvelope>
    {
        protected EventSerializerBase(bool compress = true): base(compress)
        {
        }

        protected override EventEnvelope CreateEnvelope(byte[] data, string typeName)
        {
            return new EventEnvelope
            {
                Type = typeName,
                Data = data
            };
        }
    }
}
