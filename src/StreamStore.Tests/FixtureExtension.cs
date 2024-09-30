using AutoFixture;

namespace StreamStore.Tests
{
    internal static class FixtureExtension
    {
        public static EventRecord[] CreateEventRecords(this Fixture fixture,  int initialRevision, int count)
        {
            var revision = initialRevision;

            var records =
                    fixture
                    .Build<EventRecord>()
                    .With(x => x.Revision, () => revision++)
                    .CreateMany(count)
                    .ToArray();

            return records;
        }

        public static EventRecord[] CreateEventRecords(this Fixture fixture, int count)
        {
            return CreateEventRecords(fixture, count, 1);
        }
    }
}
