
using FluentAssertions;
using StreamStore.Testing;
using StreamStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using static StreamStore.Tests.Configuration.SingleTenant.SingleTenantConfiguratorTestEnvironment;
using StreamStore.InMemory.Extensions;

namespace StreamStore.Tests.Configuration.SingleTenant
{
    public class Configuring_single_database: Scenario<SingleTenantConfiguratorTestEnvironment>
    {

        [Fact]
        public void When_database_is_not_configured()
        {
            // Arrange
            var configurator = CreateConfigurator();

            // Act
            var act = () => configurator.Configure();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend (IStreamDatabase) is not registered");
        }

        [Fact]
        public void When_database_is_configured_by_defaults()
        {
            // Arrange
            var configurator = CreateConfigurator();

            // Act
            configurator.UseInMemoryDatabase();
            var services =  configurator.Configure();

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IStreamDatabase>()
                     .Should().NotBeNull()
                     .And.BeOfType<InMemoryStreamDatabase>();

            provider.GetRequiredService<IStreamReader>()
                        .Should().NotBeNull()
                        .And.BeOfType<InMemoryStreamDatabase>();

            provider.GetRequiredService<ISchemaProvisioner>()
                    .Should().NotBeNull()
                    .And.BeOfType<DefaultSchemaProvisioner>();
        }


        [Fact]
        public void When_database_is_configured_by_database_instance()
        {
            // Arrange
            var configurator = CreateConfigurator();
            var database = MockStreamDatabase.Object;

            // Act
            configurator.UseDatabase(database);
            var services = configurator.Configure();

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IStreamDatabase>()
                     .Should().NotBeNull()
                     .And.Be(database);

            provider.GetRequiredService<IStreamReader>()
                        .Should().NotBeNull()
                        .And.Be(database);

            provider.GetRequiredService<ISchemaProvisioner>()
                    .Should().NotBeNull()
                    .And.BeOfType<DefaultSchemaProvisioner>();
        }


        [Fact]
        public void When_database_is_configured_with_custom_dependencies()
        {
            // Arrange
            var configurator = CreateConfigurator();


            // Act
            configurator
                .UseInMemoryDatabase()
                .UseSchemaProvisioner<FakeSchemaProvisioner>();
            var services = configurator.Configure();

            //Assert
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<ISchemaProvisioner>()
                     .Should().NotBeNull()
                     .And.BeOfType<FakeSchemaProvisioner>();
        }
    }
}
