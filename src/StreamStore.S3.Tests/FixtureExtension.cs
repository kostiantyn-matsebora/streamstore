using AutoFixture;
using StreamStore.Serialization;



namespace StreamStore.S3.Tests
{
    internal static class FixtureExtension
    {
        
        public static EventRecord[] CreateEventItems(this Fixture fixture,  int initialRevision, int count)
        {
            var serializer = new EventSerializer();

            var revision = initialRevision;

            var records =
                    fixture
                    .Build<EventRecord>()
                    .With(x => x.Revision, () =>revision++)
                    .With(x => x.Data, serializer.Serialize(fixture.Create<RootEvent>()))
                    .CreateMany(count)
                    .ToArray();

            return records;
        }

        public static EventRecord[] CreateEventItems(this Fixture fixture, int count)
        {
            return CreateEventItems(fixture,  1, count);
        }
    }
}
