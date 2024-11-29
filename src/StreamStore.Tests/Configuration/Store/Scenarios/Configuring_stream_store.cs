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
    public class Configuring_stream_store : Scenario<StreamStoreConfiguratorSuite>
    {
        [Fact]
        public void When_database_is_not_configured()
        {
            // Arrange
            var configurator = StreamStoreConfiguratorSuite.CreateConfigurator();

            // Act
            var act = () => configurator.Configure(StreamStoreConfiguratorSuite.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend is not registered");
        }

        [Fact]
        public void When_store_configured_in_single_mode()
        {
            // Arrange
            var database = Generated.MockOf<IStreamDatabase>();
            var typeRegistry = Generated.MockOf<ITypeRegistry>();
            var configurator = StreamStoreConfiguratorSuite.CreateConfigurator();
            var pageSize = Generated.Int;
            var mode = StreamReadingMode.ProduceConsume;
            var services = StreamStoreConfiguratorSuite.CreateServiceCollection();

            // Act
            configurator.WithReadingPageSize(pageSize);
            configurator.WithReadingMode(mode);
            configurator.WithSingleDatabase(x => x.UseInMemoryDatabase());
            configurator.EnableSchemaProvisioning();

            configurator.Configure(services);

            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<IStreamStore>().Should().NotBeNull();

            provider.GetRequiredService<IStreamDatabase>()
                        .Should().NotBeNull()
                        .And.BeOfType<InMemoryStreamDatabase>();

            provider.GetRequiredService<IStreamReader>()
                        .Should().NotBeNull()
                        .And.BeOfType<InMemoryStreamDatabase>();

            provider.GetRequiredService<StreamEventEnumeratorFactory>()
                        .Should().NotBeNull()
                        .And.BeOfType<StreamEventEnumeratorFactory>();

            provider.GetRequiredService<EventConverter>()
                        .Should().NotBeNull()
                        .And.BeOfType<EventConverter>();

            provider.GetRequiredService<EventConverter>()
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
            var database = Generated.MockOf<IStreamDatabase>();
            var typeRegistry = Generated.MockOf<ITypeRegistry>();
            var configurator = StreamStoreConfiguratorSuite.CreateConfigurator();
            var pageSize = Generated.Int;
            var mode = StreamReadingMode.ProduceConsume;
            var services = StreamStoreConfiguratorSuite.CreateServiceCollection();

            // Act
            configurator.WithReadingPageSize(pageSize);
            configurator.WithReadingMode(mode);
            configurator.WithMultitenancy(x => x.UseInMemoryDatabase());
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

            provider.GetRequiredService<EventConverter>()
                        .Should().NotBeNull()
                        .And.BeOfType<EventConverter>();

            provider.GetRequiredService<EventConverter>()
                        .Should().NotBeNull()
                        .And.BeOfType<EventConverter>();


            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration!.ReadingPageSize.Should().Be(pageSize);
            configuration.ReadingMode.Should().Be(mode);
        }
    }
}
