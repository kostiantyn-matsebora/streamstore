
using StreamStore.Testing.Framework;
using StreamStore.Tests.InMemory;

namespace StreamStore.Testing.InMemory
{
    public class InMemoryStreamDatabaseTests: StreamDatabaseTestsBase<InMemoryTestSuite>
    {
        public InMemoryStreamDatabaseTests() : base(new InMemoryTestSuite())
        {
        }
    }
}
