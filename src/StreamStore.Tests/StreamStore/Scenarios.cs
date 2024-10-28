using StreamStore.Testing.StreamStore.Scenarios;

namespace StreamStore.Tests.StreamStore
{
    public class Reading_from_stream : Reading_from_stream<InMemoryStreamStoreSuite>
    {
        public Reading_from_stream() : base(new InMemoryStreamStoreSuite())
        {
        }
    }

    public class Writing_to_stream : Writing_to_stream<InMemoryStreamStoreSuite>
    {
        public Writing_to_stream() : base(new InMemoryStreamStoreSuite())
        {
        }
    }

    public class Deleting_stream : Deleting_stream<InMemoryStreamStoreSuite>
    {
        public Deleting_stream() : base(new InMemoryStreamStoreSuite())
        {
        }
    }
}
