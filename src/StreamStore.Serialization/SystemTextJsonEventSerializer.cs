using System;
using System.Text;

namespace StreamStore.Serialization
{
    public sealed  class SystemTextJsonEventSerializer: EventSerializerBase
    {
        public SystemTextJsonEventSerializer(bool compress = true): base(compress)
        {
            
        }
        protected override byte[] SerializeObject(object @event, Type type)
        {
            return Encoding.UTF8.GetBytes(
                System.Text.Json.JsonSerializer.Serialize(@event));
        }

        protected override object DeserializeObject(byte[] data, Type type)
        {
            return System.Text.Json.JsonSerializer.Deserialize(
                Encoding.UTF8.GetString(data), type)!;
        }
    }
}
