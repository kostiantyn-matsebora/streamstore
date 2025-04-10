using AutoFixture;
namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class StreamEventRecords
        {
            public static IStreamEventRecord[] Many(int initialRevision, int count)
            {
                return new Fixture().CreateStreamEventRecords(initialRevision, count);
            }

            public static IStreamEventRecord[] Many(int count)
            {
                return new Fixture().CreateStreamEventRecords(count);
            }
        }
    }
}
