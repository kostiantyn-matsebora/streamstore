using AutoFixture;

namespace StreamStore.Tests
{
    internal static class FixtureExtension
    {
        public static EventRecord[] CreateEvents(this Fixture fixture, int count, int initialRevision)
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
    }
}
