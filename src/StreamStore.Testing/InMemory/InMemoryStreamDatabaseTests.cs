
namespace StreamStore.Testing.InMemory
{
    public class InMemoryStreamDatabaseTests: DatabaseIntegrationTestsBase
    {
        public InMemoryStreamDatabaseTests() : base(new InMemoryTestSuite())
        {
        }
    }
}
