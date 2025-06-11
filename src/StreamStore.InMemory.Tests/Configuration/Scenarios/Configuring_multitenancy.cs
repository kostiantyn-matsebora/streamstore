using StreamStore.InMemory.Configuration;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.InMemory.Tests.Configuration
{
    public class Configuring_multitenancy: MultitenancyConfiguratorScenario
    {
        protected override IMultitenancyConfigurator CreateConfigurator()
        {
            return new MultitenancyConfigurator();
        }
    }
}
