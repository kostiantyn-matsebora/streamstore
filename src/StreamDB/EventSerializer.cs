
namespace StreamDB
{
    internal class EventSerializer
    {
        public static string Serialize(object @event)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(@event);
        }

        public static object Deserialize(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(data);
        }
    }
}
