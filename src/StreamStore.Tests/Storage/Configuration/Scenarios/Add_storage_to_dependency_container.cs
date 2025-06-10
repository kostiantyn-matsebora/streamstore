

using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing;
using StreamStore.Storage;
using FluentAssertions;
using StreamStore.Storage.Configuration;


namespace StreamStore.Tests.Storage.Configuration
{
    public class Add_storage_to_dependency_container : Scenario
    {
        [Fact]
        public void When_configurator_is_not_defined()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var act = () => services.ConfigurePersistence(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("configurator");

            // Act
            act = () => services.ConfigurePersistenceMultitenancy(null!, Generated.Mocks.Single<MultitenancyConfiguratorBase>().Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("configurator");

            // Act
            act = () => services.ConfigurePersistenceMultitenancy(Generated.Mocks.Single<StorageConfiguratorBase>().Object, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("multitenancyConfigurator");
        }

        [Fact]
        public void When_configurator_is_defined()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = Generated.Mocks.Single<StorageConfiguratorBase>();
            var multitenancyConfigurator = Generated.Mocks.Single<MultitenancyConfiguratorBase>();

            // Act
            var result = services.ConfigurePersistence(configurator.Object);

            // Assert
            result.Should().NotBeNull();

            // Act
            result = services.ConfigurePersistenceMultitenancy(configurator.Object, multitenancyConfigurator.Object);

            // Assert
            result.Should().NotBeNull();

        }

    }
}
