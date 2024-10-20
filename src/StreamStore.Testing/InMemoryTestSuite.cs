using StreamStore.InMemory;

namespace StreamStore.Testing
{
    internal class InMemoryTestSuite : ITestSuite
    {
        public bool IsReady => true;
      
        public void Initialize()
        {
        }

        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            await action(CreateDatabase()!);
        }

        static IStreamDatabase? CreateDatabase()
        {
            return new InMemoryStreamDatabase();
        }

    }
}
