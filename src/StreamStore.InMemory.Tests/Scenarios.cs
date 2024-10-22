using StreamStore.Testing.Scenarios.StreamDatabase;
using Xunit.Abstractions;

namespace StreamStore.InMemory.Tests
{
    public class Finding_stream_metadata : Find_stream_data<InMemoryTestSuite>
    {
        public Finding_stream_metadata() : base(new InMemoryTestSuite())
        {
        }
    }
    public class Reading_from_database : Reading_from_database<InMemoryTestSuite>
    {
        public Reading_from_database(ITestOutputHelper output) : base(new InMemoryTestSuite(), output)
        {
        }
    }
}
