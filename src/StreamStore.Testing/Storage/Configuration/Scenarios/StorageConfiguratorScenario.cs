using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Storage;

namespace StreamStore.Testing.Storage.Configuration
{
    public  abstract class StorageConfiguratorScenario
    {
        protected abstract IStorageConfigurator CreateConfigurator();

        protected virtual void ConfigureRequiredDependencies(IServiceCollection services)
        {
        }

        [Fact]
        public void When_configuring_storage()
        {
            // Arrange
            var configurator = CreateConfigurator();
            var services = new ServiceCollection();
            
            // Act
            ConfigureRequiredDependencies(services);
            configurator.Configure(services);
            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetService<IStreamStorage>().Should().NotBeNull();
            provider.GetService<ISchemaProvisioner>().Should().NotBeNull();
        }
    }
}
