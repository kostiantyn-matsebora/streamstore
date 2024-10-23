using AutoFixture;
using StreamStore.Serialization;


namespace StreamStore.Testing
{
    public static class Generated
    {
        public static string String => new Fixture().Create<string>();

        public static Id Id => new Fixture().Create<Id>();
        public static DateTime DateTime => DateTime.UtcNow;
        public static int Int
        {
            get
            {
                return new Random().Next(Int32.MinValue, Int32.MaxValue);
            }
        }

        public static Revision Revision
        {
            get
            {
                return Revision.New(new Random().Next(0, Int32.MaxValue));
            }
        }

        public static byte[] ByteArray => Converter.ToByteArray(String);

        public static EventItem EventItem => new Fixture().CreateEventItems(1).First();

        public static RootEvent Event => new Fixture().CreateEvents(1).First();

        public static EventItem[] CreateEventItems(int count)
        {
            return new Fixture().CreateEventItems(count);
        }
    }
}
