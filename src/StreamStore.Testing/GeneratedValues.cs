using AutoFixture;
using StreamStore.S3;


namespace StreamStore.Testing
{
    public static class GeneratedValues
    {
        public static string String => new Fixture().Create<string>();

        public static DateTime DateTime => DateTime.UtcNow;
        public static int Int
        {
            get
            {
                return new Random().Next(Int32.MinValue, Int32.MaxValue);
            }
        }

        public static byte[] ByteArray => Converter.ToByteArray(String);

        public static EventItem[] CreateEventItems(int count)
        {
            return new Fixture().CreateEventItems(count);
        }
    }
}
