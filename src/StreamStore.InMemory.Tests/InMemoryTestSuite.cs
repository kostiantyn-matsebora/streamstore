using StreamStore.InMemory;
using StreamStore.Testing.Scenarios.StreamDatabase;

namespace StreamStore.InMemory.Tests
{
    public class InMemoryTestSuite : DatabaseSuiteBase
    {
        public InMemoryTestSuite()
        {
        }

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseMemoryStreamDatabase();
        }
    }
}
