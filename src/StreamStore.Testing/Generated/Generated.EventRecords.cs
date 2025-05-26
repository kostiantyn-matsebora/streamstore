using System.Linq;
using AutoFixture;


namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class EventRecords
        {
            public static IStreamEventRecord Single => new Fixture().CreateStreamEventRecords(1).First();

        }
    }
}
