using StreamStore.InMemory;

namespace StreamStore.Testing.InMemory
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

        static InMemoryStreamDatabase? CreateDatabase()
        {
            return new InMemoryStreamDatabase();
        }

    }
}
