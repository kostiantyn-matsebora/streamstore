using System.Text;

namespace StreamStore.S3
{
    internal class Converter
    {

        public static byte[] ToByteArray(object obj)
        {
            return Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }

        public static T? FromByteArray<T>(byte[] data) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }
    }
}
