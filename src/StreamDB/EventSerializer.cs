
using System;

namespace StreamDB
{
    internal class EventSerializer : IEventSerializer
    {
        public string Serialize(object @event)
        {
            if (@event == null)
                throw new System.ArgumentNullException(nameof(@event));

            return Newtonsoft.Json.JsonConvert.SerializeObject(@event);
        }

        public object Deserialize(string data)
        {
            if (data == null)
                throw new System.ArgumentNullException(nameof(data));

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            if (result == null)
                throw new ArgumentException("Cannot deserialize event", nameof(data));

            return result;
        }
    }
}
