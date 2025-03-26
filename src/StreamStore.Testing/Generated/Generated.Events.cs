using System.Linq;
using AutoFixture;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class Events
        {
            public static RootEvent Single => new Fixture().CreateEvents(1).First();


            public static Event[] Many(int count)
            {
                return new Fixture().CreateMany<Event>(count).ToArray();
            }
        }
    }
}
