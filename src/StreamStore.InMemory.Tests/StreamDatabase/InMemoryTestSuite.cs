using StreamStore.InMemory.Extensions;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.InMemory.Tests.StreamDatabase
{
    public class InMemoryTestSuite : DatabaseSuiteBase
    {
        public InMemoryTestSuite()
        {
        }

        protected override void ConfigureDatabase(ISingleTenantDatabaseConfigurator configurator)
        {
            configurator.UseInMemoryDatabase();
        }
    }
}
