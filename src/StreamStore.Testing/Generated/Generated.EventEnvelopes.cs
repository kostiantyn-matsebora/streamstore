using System.Linq;
using AutoFixture;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class EventEnvelopes
        {
            public static TestEventEnvelope Single => new Fixture().Create<TestEventEnvelope>();


            public static TestEventEnvelope[] Many(int count)
            {
                return new Fixture().CreateMany<TestEventEnvelope>(count).ToArray();
            }
        }
    }
}
