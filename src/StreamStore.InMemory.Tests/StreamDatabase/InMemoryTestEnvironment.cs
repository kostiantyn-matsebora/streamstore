using StreamStore.InMemory.Extensions;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.InMemory.Tests.StreamDatabase
{
    public class InMemoryTestEnvironment : DatabaseTestEnvironmentBase
    {
        public InMemoryTestEnvironment()
        {
        }

        protected override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
            configurator.UseInMemoryDatabase();
        }
    }
}
