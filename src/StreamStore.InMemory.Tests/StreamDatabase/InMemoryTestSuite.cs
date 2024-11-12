using StreamStore.Testing.StreamDatabase;

namespace StreamStore.InMemory.Tests.StreamDatabase
{
    public class InMemoryTestSuite : DatabaseSuiteBase
    {
        public InMemoryTestSuite()
        {
        }

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.WithSingleTenant(c => c.UseInMemoryDatabase());
        }
    }
}
