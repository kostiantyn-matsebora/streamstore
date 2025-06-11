using StreamStore.Testing.StreamStore.Scenarios;
using StreamStore.Tests.StreamStore.Scenarios;

namespace StreamStore.Tests.StreamStore
{
    public class Reading_from_stream : Reading_from_stream<InMemoryStreamStoreTestEnvironment>
    {
        public Reading_from_stream() : base(new InMemoryStreamStoreTestEnvironment())
        {
        }
    }

    public class Writing_to_stream : Writing_to_stream<InMemoryStreamStoreTestEnvironment>
    {
        public Writing_to_stream() : base(new InMemoryStreamStoreTestEnvironment())
        {
        }
    }

    public class Deleting_stream : Deleting_stream<InMemoryStreamStoreTestEnvironment>
    {
        public Deleting_stream() : base(new InMemoryStreamStoreTestEnvironment())
        {
        }
    }

    public class Getting_stream_metadata : Getting_stream_metadata<InMemoryStreamStoreTestEnvironment>
    {
        public Getting_stream_metadata() : base(new InMemoryStreamStoreTestEnvironment())
        {
        }
    }
}
