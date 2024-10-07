
namespace StreamStore.Testing.InMemory
{
    public class InMemoryStreamDatabaseTests: StreamDatabaseTestsBase
    {
        public InMemoryStreamDatabaseTests() : base(new InMemoryTestSuite())
        {
        }
    }
}
