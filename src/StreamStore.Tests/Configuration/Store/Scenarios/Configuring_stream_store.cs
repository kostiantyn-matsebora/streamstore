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
        public void When_storage_is_not_configured()
        {
            // Arrange
            var configurator = StreamStoreConfiguratorTestEnvironment.CreateConfigurator();

            // Act
            var act = () => configurator.Configure(StreamStoreConfiguratorTestEnvironment.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Storage backend is not registered");
        }

        [Fact]
        public void When_store_configured_in_single_mode()
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
            configurator.WithSingleStorage(x => x.UseInMemoryStorage());
            configurator.EnableSchemaProvisioning();

            configurator.Configure(services);

            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<IStreamStore>().Should().NotBeNull();

            provider.GetRequiredService<IStreamStorage>()
                        .Should().NotBeNull()
                        .And.BeOfType<InMemoryStreamStorage>();

            provider.GetRequiredService<IStreamReader>()
                        .Should().NotBeNull()
                        .And.BeOfType<InMemoryStreamStorage>();

            provider.GetRequiredService<StreamEventEnumeratorFactory>()
                        .Should().NotBeNull()
                        .And.BeOfType<StreamEventEnumeratorFactory>();

            provider.GetRequiredService<IEventConverter>()
                        .Should().NotBeNull()
                        .And.BeOfType<EventConverter>();

            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration!.ReadingPageSize.Should().Be(pageSize);
            configuration.ReadingMode.Should().Be(mode);
        }

        [Fact]
        public void When_store_configured_in_multitenant_mode()
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
            configurator.WithMultitenancy(x => x.UseInMemoryStorage());
            configurator.EnableSchemaProvisioning();

            configurator.Configure(services);

            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<ITenantStreamStoreFactory>().Should().NotBeNull();

            provider.GetRequiredService<ITenantSchemaProvisionerFactory>()
                        .Should().NotBeNull()
                        .And.BeOfType<DefaultSchemaProvisionerFactory>();

            provider.GetRequiredService<ITenantProvider>()
                        .Should().NotBeNull()
                        .And.BeOfType<DefaultTenantProvider>();

            provider.GetRequiredService<IEventConverter>()
                        .Should().NotBeNull()
                        .And.BeOfType<EventConverter>();

            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration!.ReadingPageSize.Should().Be(pageSize);
            configuration.ReadingMode.Should().Be(mode);
        }
    }
}
