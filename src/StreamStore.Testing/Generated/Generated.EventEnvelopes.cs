using System.Linq;
using AutoFixture;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class EventEnvelopes
        {
            public static RootEvent Single => new Fixture().CreateEvents(1).First();


            public static TestEventEnvelope[] Many(int count)
            {
                return new Fixture().CreateMany<TestEventEnvelope>(count).ToArray();
            }
        }
    }
}
