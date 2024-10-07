using StreamStore.InMemory;

namespace StreamStore.Testing
{
    internal class InMemoryTestSuite : ITestSuite
    {
        public IStreamDatabase CreateDatabase()
        {
            return new InMemoryStreamDatabase();
        }
    }
}
