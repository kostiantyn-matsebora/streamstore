using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StreamStore.Configuration;
using StreamStore.InMemory.Extensions;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Storage;
using StreamStore.Testing;

namespace StreamStore.Tests.StreamStore.Configuration
{
    public class Configure_store : Scenario
    {
        [Fact]
        public void When_storage_is_not_configured()
        {
            // Arrange
            var configurator = new StreamStoreConfigurator();
            var servces = new ServiceCollection();
            // Act
            var act = () => configurator.Configure(servces);
            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_provisioning_is_enabled()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var provider =
              new StreamStoreConfigurator()
             .EnableAutomaticProvisioning()
             .ConfigurePersistence(s => s.AddInMemoryStorage())
             .Configure(services)
             .BuildServiceProvider();


            // Assert
            provider.GetService<IHostedService>()
                    .Should().NotBeNull()
                    .And.BeOfType<SchemaProvisioningService>();

        }

        [Fact]
        public void When_multitenancy_is_enabled()
        {
            // Arrange
            var services = new ServiceCollection();
            // Act
            var provider =
              new StreamStoreConfigurator()
             .EnableMultitenancy(Generated.Primitives.Id, Generated.Primitives.Id)
             .EnableAutomaticProvisioning()
             .ConfigurePersistence(s => s.AddInMemoryStorageWithMultitenancy())
             .Configure(services)
             .BuildServiceProvider();

            // Assert
            provider.GetService<IHostedService>()
                    .Should().NotBeNull()
                    .And.BeOfType<TenantSchemaProvisioningService>();

            provider.GetService<ITenantProvider>().Should().NotBeNull();
            provider.GetService<ITenantStreamStoreFactory>().Should().NotBeNull();
        }

        [Fact]
        public void When_tenant_provider_is_set()
        {
            // Arrange
            var services = new ServiceCollection();
            // Act
            var provider =
              new StreamStoreConfigurator()
             .EnableMultitenancy<TenantProvider>()
             .EnableAutomaticProvisioning()
             .ConfigurePersistence(s => s.AddInMemoryStorageWithMultitenancy())
             .Configure(services)
             .BuildServiceProvider();

            // Assert
            provider.GetService<IHostedService>()
                    .Should().NotBeNull()
                    .And.BeOfType<TenantSchemaProvisioningService>();
            var configuration = provider.GetService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration.MultitenancyEnabled.Should().BeTrue();

            provider.GetService<ITenantProvider>().Should().NotBeNull();
            provider.GetService<ITenantStreamStoreFactory>().Should().NotBeNull();
        }

        [Fact]
        public void When_tenants_are_not_provided()
        {
            // Act
            var act = () => new StreamStoreConfigurator().EnableMultitenancy();

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Tenants cannot be null or empty.*")
                .And.ParamName.Should().Be("tenants");

        }

        [Fact]
        public void When_configuration_parameters_are_changed()
        {
            // Arrange

            IServiceCollection services = new ServiceCollection();
            var readingPageSize = Generated.Primitives.Int;
            var configurator = new StreamStoreConfigurator().ConfigurePersistence(s => s.AddInMemoryStorageWithMultitenancy());

            // Act
            services =
                configurator
                .EnableMultitenancy(Generated.Primitives.Id, Generated.Primitives.Id)
                .WithReadingPageSize(readingPageSize)
                .WithReadingMode(StreamReadingMode.ProduceConsume)
                .Configure(services);


            // Assert
            services.BuildServiceProvider()
                .GetService<StreamStoreConfiguration>()
                .Should().NotBeNull()
                .And.Match<StreamStoreConfiguration>(c =>
                    c.ReadingPageSize == readingPageSize &&
                    c.ReadingMode == StreamReadingMode.ProduceConsume);
        }

        class TenantProvider : ITenantProvider
        {
            public IEnumerable<Id> GetAll()
            {
                return new[] { Generated.Primitives.Id, Generated.Primitives.Id };
            }
        }
    }
}
