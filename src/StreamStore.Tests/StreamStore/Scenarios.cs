using StreamStore.Testing.StreamStore.Scenarios;

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
}
