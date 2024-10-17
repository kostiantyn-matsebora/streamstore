using System;

namespace StreamStore.Serialization
{
    public sealed class NewtonsoftEventSerializer : EventSerializerBase
    {
        public NewtonsoftEventSerializer(bool compress = true) : base(compress)
        {
        }

        protected override byte[] SerializeObject(object @event, Type type)
        {
           return System.Text.Encoding.UTF8.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(@event));
        }

        protected override object DeserializeObject(byte[] data, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(
                        System.Text.Encoding.UTF8.GetString(data), type)!;
        }
    }
}
