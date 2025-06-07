using System.Linq;
using AutoFixture;
using StreamStore.Testing.Models;
namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class StreamEventRecords
        {
            public static TestStreamEventRecord[] Many(int initialRevision, int count)
            {
                return new Fixture().CreateStreamEventRecords(initialRevision, count).ToArray();
            }

            public static TestStreamEventRecord[] Many(int count)
            {
                return new Fixture().CreateStreamEventRecords(count).ToArray();
            }

            public static TestStreamEventRecord[] Single => new Fixture().CreateStreamEventRecords(1,1).ToArray();
           
        }
    }
}
