using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using StreamStore.InMemory.Extensions;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Serialization;
using StreamStore.Testing;

namespace StreamStore.Tests.Configuration.Store
{
    public class Configuring_stream_store : Scenario<StreamStoreConfiguratorTestEnvironment>
    {

        [Fact]
        public void When_store_configured()
        {
            // Arrange
            var storage = Generated.Mocks.Single<IStreamStorage>();
            var typeRegistry = Generated.Mocks.Single<ITypeRegistry>();
            var configurator = StreamStoreConfiguratorTestEnvironment.CreateConfigurator();
            var pageSize = Generated.Primitives.Int;
            var mode = StreamReadingMode.ProduceConsume;
            var services = StreamStoreConfiguratorTestEnvironment.CreateServiceCollection();

            // Act
            configurator.WithReadingPageSize(pageSize);
            configurator.WithReadingMode(mode);
            configurator.ConfigurePersistence(x => x.AddInMemoryStorage());
            configurator.EnableAutomaticProvisioning();
            services = configurator.Configure(services);

            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<IStreamStore>().Should().NotBeNull();

            provider.GetRequiredService<IStreamStorage>()
                        .Should().NotBeNull();


            provider.GetRequiredService<IStreamReader>()
                        .Should().NotBeNull();


            provider.GetRequiredService<StreamEventEnumeratorFactory>()
                        .Should().NotBeNull();


            provider.GetRequiredService<IEventConverter>()
                        .Should().NotBeNull();
                        

            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration!.ReadingPageSize.Should().Be(pageSize);
            configuration.ReadingMode.Should().Be(mode);
        }

        [Fact]
        public void When_store_configured_with_multitenancy()
        {
            // Arrange
            var storage = Generated.Mocks.Single<IStreamStorage>();
            var typeRegistry = Generated.Mocks.Single<ITypeRegistry>();
            var configurator = StreamStoreConfiguratorTestEnvironment.CreateConfigurator();
            var pageSize = Generated.Primitives.Int;
            var mode = StreamReadingMode.ProduceConsume;
            var services = StreamStoreConfiguratorTestEnvironment.CreateServiceCollection();

            // Act
            configurator.WithReadingPageSize(pageSize);
            configurator.WithReadingMode(mode);
            configurator.EnableMultitenancy(Generated.Primitives.Id);
            configurator.ConfigurePersistence(x => x.AddInMemoryStorageWithMultitenancy());
            configurator.EnableAutomaticProvisioning();
            services = configurator.Configure(services);

            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<ITenantStreamStoreFactory>().Should().NotBeNull();

            provider.GetRequiredService<ITenantSchemaProvisionerFactory>()
                        .Should().NotBeNull();

            provider.GetRequiredService<ITenantProvider>()
                        .Should().NotBeNull();


            provider.GetRequiredService<IEventConverter>()
                        .Should().NotBeNull();
                        

            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration!.ReadingPageSize.Should().Be(pageSize);
            configuration.ReadingMode.Should().Be(mode);
        }
    }
}
