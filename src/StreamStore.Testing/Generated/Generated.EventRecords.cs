using AutoFixture;
using StreamStore.Storage;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static StreamEventRecord[] Many(int initialRevision, int count)
        {
            return new Fixture().CreateEventRecords(initialRevision, count);
        }

        public static StreamEventRecord[] Many(int count)
        {
            return new Fixture().CreateEventRecords(count);
        }
    }
}
