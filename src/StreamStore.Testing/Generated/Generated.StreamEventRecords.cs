using System.Linq;
using AutoFixture;
using StreamStore.Storage;
namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class StreamEventRecords
        {
            public static StreamEventRecord[] Many(int initialRevision, int count)
            {
                return new Fixture().CreateStreamEventRecords(initialRevision, count).ToArray();
            }

            public static StreamEventRecord[] Many(int count)
            {
                return new Fixture().CreateStreamEventRecords(count).ToArray();
            }

            public static StreamEventRecord[] Single => new Fixture().CreateStreamEventRecords(1,1).ToArray();
           
        }
    }
}
