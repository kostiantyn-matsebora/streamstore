using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.InMemory
{
    internal class S3InMemorySuite : ITestSuite
    {
        public bool IsReady => true;

        public void Initialize()
        {
        }

        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            var database = CreateDatabase();
            await action(database!);
        }

        static S3StreamDatabase? CreateDatabase()
        {
            return new S3StreamDatabase(new S3InMemoryFactory(), new S3StorageFactory(new S3InMemoryFactory()));
        }
    }
}
