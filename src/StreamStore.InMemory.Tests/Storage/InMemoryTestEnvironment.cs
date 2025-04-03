using StreamStore.InMemory.Extensions;
using StreamStore.Testing.StreamStorage;

namespace StreamStore.InMemory.Tests.StreamStorage
{
    public class InMemoryTestEnvironment : StorageTestEnvironmentBase
    {
        public InMemoryTestEnvironment()
        {
        }

        protected override void ConfigureStorage(ISingleTenantConfigurator configurator)
        {
            configurator.UseInMemoryStorage();
        }
    }
}
