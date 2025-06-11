using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage.Configuration;
using StreamStore.Testing;

namespace StreamStore.Tests.Storage.Configuration
{
    public class Configuring_multitenancy
    {
        [Fact]
        public void When_services_is_not_defined()
        {

            // Arrange
            var configurator = Generated.Mocks.Single<MultitenancyConfiguratorBase>();
            // Act
            var act = () => configurator.Object.Configure(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("services");
        }

        [Fact]
        public void When_services_is_defined()
        {
            // Arrange
            var configurator = new MultitenancyConfigurator();


            var services = new ServiceCollection();

            // Act
            configurator.Configure(services);

            // Assert
            configurator.Validate();
        }

        class MultitenancyConfigurator : MultitenancyConfiguratorBase
        {
            bool configureAdditionalDependenciesCalled;
            bool configureSchemaProvisionerFactoryCalled;
            bool configureStorageProviderCalled;

            protected override void ConfigureAdditionalDependencies(IServiceCollection services)
            {
                configureAdditionalDependenciesCalled = true;
            }

            protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
            {
                configureSchemaProvisionerFactoryCalled = true;
            }

            protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
            {
                configureStorageProviderCalled = true;
            }

            public void Validate()
            {
                configureAdditionalDependenciesCalled.Should().BeTrue("ConfigureAdditionalDependencies should have been called");
                configureSchemaProvisionerFactoryCalled.Should().BeTrue("ConfigureSchemaProvisionerFactory should have been called");
                configureStorageProviderCalled.Should().BeTrue("ConfigureStorageProvider should have been called");
            }
        }
    }
}
