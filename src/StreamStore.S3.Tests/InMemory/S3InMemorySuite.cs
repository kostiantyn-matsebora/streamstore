using StreamStore.Testing;

namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemorySuite : ITestSuite
    {
        public IStreamDatabase? CreateDatabase()
        {
            return new S3StreamDatabase(new S3InMemoryFactory());
        }
    }
}
