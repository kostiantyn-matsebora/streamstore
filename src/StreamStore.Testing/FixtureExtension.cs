using AutoFixture;
using StreamStore.Serialization;


namespace StreamStore.Testing
{
    public static class FixtureExtension
    {

        public static EventItem[] CreateEventItems(this Fixture fixture, int count)
        {
            var serializer = new NewtonsoftEventSerializer();


            var records =
                    fixture
                    .Build<EventItem>()
                    .With(x => x.Data, serializer.Serialize(fixture.Create<RootEvent>()))
                    .CreateMany(count)
                    .ToArray();

            return records;
        }

        public static EventRecord[] CreateEventRecords(this Fixture fixture, int initialRevision, int count)
        {
            var serializer = new NewtonsoftEventSerializer();

            var revision = initialRevision;

            var records =
                    fixture
                    .Build<EventRecord>()
                    .With(x => x.Revision, () => revision++)
                    .With(x => x.Data, serializer.Serialize(fixture.Create<RootEvent>()))
                    .CreateMany(count)
                    .ToArray();

            return records;
        }

        public static EventRecord[] CreateEventRecords(this Fixture fixture,  int count)
        {
            return CreateEventRecords(fixture, 1, count);
        }
    }
}
