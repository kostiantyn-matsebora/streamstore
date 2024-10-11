using AutoFixture;
using StreamStore.S3;
using StreamStore.Serialization;

namespace StreamStore.Testing
{
    public static  class GeneratedValues
    {
        public static string String => new Fixture().Create<string>();

        public static byte[] ByteArray => Converter.ToByteArray(String);

        public static EventItem[] CreateEventItems(int count)
        {
            return new Fixture().CreateEventItems(count);
        }
    }
}
