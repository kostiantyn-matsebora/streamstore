using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Storage;

namespace StreamStore.Testing.Storage.Configuration { 
    public  abstract class MultitenancyConfiguratorScenario: Scenario
    {

        protected abstract IMultitenancyConfigurator CreateConfigurator();

        protected virtual void ConfigureRequiredDependencies(IServiceCollection services)
        {
        }

        [Fact]
        public void When_configuring_multitenancy()
        {
            // Arrange
            var configurator = CreateConfigurator();
            var services = new ServiceCollection();
            
            // Act
            ConfigureRequiredDependencies(services);
            configurator.Configure(services);
            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetService<ITenantStreamStorageProvider>().Should().NotBeNull();
            provider.GetService<ITenantSchemaProvisionerFactory>().Should().NotBeNull();

        }
    }
}
