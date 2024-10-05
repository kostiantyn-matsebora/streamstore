using System.Text;

namespace StreamStore.S3
{
    internal class Converter
    {

        public static string ToString(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T? FromString<T>(string data) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
    }
}
