using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using StreamStore.InMemory.Extensions;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Testing;
using static StreamStore.Tests.Configuration.MultiTenant.MultiTenantConfiguratorSuite;


namespace StreamStore.Tests.Configuration.MultiTenant
{
    public class Configuring_multitenancy: Scenario<MultiTenantConfiguratorSuite>
    {

        [Fact]
        public void When_database_provider_is_not_configured()
        {
            // Arrange
            var configurator = CreateConfigurator();

            // Act
            var act = configurator.Configure;

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend (ITenantStreamDatabaseProvider) is not registered");
        }

        [Fact]
        public void When_multitenancy_is_configured_by_defaults()
        {
            // Arrange
            var configurator = CreateConfigurator();

            // Act
            configurator.UseInMemoryDatabase();
        
            var services = configurator.Configure();

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ITenantStreamDatabaseProvider>()
                     .Should().NotBeNull()
                     .And.BeOfType<InMemoryStreamDatabaseProvider>();

            provider.GetRequiredService<ITenantSchemaProvisionerFactory>()
                    .Should().NotBeNull()
                    .And.BeOfType<DefaultSchemaProvisionerFactory>();
            provider.GetRequiredService<ITenantProvider>()
                    .Should().NotBeNull()
                    .And.BeOfType<DefaultTenantProvider>();
        }

        [Fact]
        public void When_multitenancy_is_configured_with_custom_dependencies()
        {
            // Arrange
            var configurator = CreateConfigurator();

            // Act
            configurator
                .UseInMemoryDatabase()
                .UseSchemaProvisionerFactory<FakeSchemaProvisionerFactory>()
                .UseTenantProvider<FakeTenantProvider>();

            var services = configurator.Configure();

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ITenantStreamDatabaseProvider>()
                     .Should().NotBeNull()
                     .And.BeOfType<InMemoryStreamDatabaseProvider>();

            provider.GetRequiredService<ITenantSchemaProvisionerFactory>()
                    .Should().NotBeNull()
                    .And.BeOfType<FakeSchemaProvisionerFactory>();

            provider.GetRequiredService<ITenantProvider>()
                .Should().NotBeNull()
                .And.BeOfType<FakeTenantProvider>();
        }
    }
}
