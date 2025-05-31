using StreamStore.Testing.StreamStorage;
using StreamStore.InMemory.Extensions;

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
