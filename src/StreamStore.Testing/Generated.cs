using AutoFixture;
using Moq;
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
                return new Revision(new Random().Next(0, Int32.MaxValue));
            }
        }

        public static byte[] ByteArray => Converter.ToByteArray(String);

        public static RootEvent Event => new Fixture().CreateEvents(1).First();


        public static EventRecord[] EventRecords(int initialRevision, int count)
        {
            return new Fixture().CreateEventRecords(initialRevision, count);
        }

        public static EventRecord[] EventRecords(int count)
        {
            return new Fixture().CreateEventRecords(count);
        }

        public static Event[] Events(int count)
        {
            return new Fixture().CreateMany<Event>(count).ToArray();
        }

        public static Mock<T> MockOf<T>() where T : class
        {
            return new Mock<T>();
        }

        public static T Object<T>() => new Fixture().Create<T>();
    }
}
