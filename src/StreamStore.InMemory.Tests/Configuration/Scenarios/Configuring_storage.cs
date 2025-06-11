using StreamStore.InMemory.Configuration;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.InMemory.Tests.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            return new StorageConfigurator();
        }
    }
}
