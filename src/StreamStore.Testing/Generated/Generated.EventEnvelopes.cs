using System.Collections.Generic;
using System.Linq;
using AutoFixture;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class EventEnvelopes
        {
            public static TestEventEnvelope Single =>  Many(1).Single();

            public static TestEventEnvelope[] Many(int count)
            {
                var fixture = new Fixture();
                var records =
                        new Fixture()
                        .Build<TestEventEnvelope>()
                        .With(x => x.CustomProperties, fixture.Create<Dictionary<string,string>>())
                        .CreateMany(count)
                        .ToArray();

                return records;
            }
        }
    }
}
