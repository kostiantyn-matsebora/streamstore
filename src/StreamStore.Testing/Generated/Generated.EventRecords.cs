using AutoFixture;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static EventRecord[] Many(int initialRevision, int count)
        {
            return new Fixture().CreateEventRecords(initialRevision, count);
        }

        public static EventRecord[] Many(int count)
        {
            return new Fixture().CreateEventRecords(count);
        }
    }
}
