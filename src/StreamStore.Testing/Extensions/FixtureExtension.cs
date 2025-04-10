using System.Linq;
using AutoFixture;
using StreamStore.Serialization;
using StreamStore.Storage;


namespace StreamStore.Testing
{
    internal static class FixtureExtension
    {
        readonly static TypeRegistry registry  = new TypeRegistry();

        public static StreamEventRecord[] CreateStreamEventRecords(this Fixture fixture, int initialRevision, int count)
        {
            var serializer = new NewtonsoftEventSerializer(registry, false);

            var revision = initialRevision;

            var records =
                    fixture
                    .Build<StreamEventRecord>()
                    .With(x => x.Revision, () => revision++)
                    .With(x => x.Data, serializer.Serialize(CreateEvents(fixture, 1).First()))
                    .CreateMany(count)
                    .ToArray();

            return records;
        }

        public static RootEvent[] CreateEvents(this Fixture fixture, int count)
        {
            return fixture.CreateMany<RootEvent>().ToArray();
        }

        public static StreamEventRecord[] CreateStreamEventRecords(this Fixture fixture,  int count)
        {
            return CreateStreamEventRecords(fixture, 1, count);
        }
    }
}
