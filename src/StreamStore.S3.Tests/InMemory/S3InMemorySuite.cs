using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemorySuite : ITestSuite
    {
        public IStreamDatabase? CreateDatabase()
        {
            return new S3StreamDatabase(new S3InMemoryFactory(), new S3StorageFactory(new S3InMemoryFactory()));
        }
    }
}
