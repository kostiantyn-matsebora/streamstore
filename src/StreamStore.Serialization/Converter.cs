using System.Text;

namespace StreamStore.Serialization
{
    public static class Converter
    {
        public static byte[] ToByteArray(object obj)
        {
            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public static T? FromByteArray<T>(byte[] data) where T : class
        {
            var encoded = Encoding.UTF8.GetString(data);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(encoded);
        }
    }
}
