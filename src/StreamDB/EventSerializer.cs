
namespace StreamDB
{
    internal class EventSerializer: IEventSerializer
    {
        public string Serialize(object @event)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(@event);
        }

        public object Deserialize(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(data);
        }
    }
}
