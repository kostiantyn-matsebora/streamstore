using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using StreamStore.Serialization;
using StreamStore.Testing;

namespace StreamStore.Tests.Configurator
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
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend is not set");
        }

        [Fact]
        public void When_store_configured_by_types()
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
            configurator.EnableCompression();
            configurator.WithEventSerializer<SystemTextJsonEventSerializer>();
            configurator.WithTypeRegistry<TypeRegistry>();
            configurator.WithSingleTenant(x => x.UseInMemoryDatabase());

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

            provider.GetRequiredService<IEventSerializer>()
                        .Should().NotBeNull()
                        .And.BeOfType<SystemTextJsonEventSerializer>();

            provider.GetRequiredService<ITypeRegistry>()
                        .Should().NotBeNull()
                        .And.BeOfType<TypeRegistry>();

            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
            configuration!.ReadingPageSize.Should().Be(pageSize);
            configuration.ReadingMode.Should().Be(mode);
            configuration.CompressionEnabled.Should().BeTrue();
        }

        [Fact]
        public void When_store_configured_by_instances()
        {
            // Arrange
            var database = Generated.MockOf<IStreamDatabase>();
            var typeRegistry = Generated.MockOf<ITypeRegistry>();
            var serializer = Generated.MockOf<IEventSerializer>();
            var configurator = StreamStoreConfiguratorSuite.CreateConfigurator();
            var services = StreamStoreConfiguratorSuite.CreateServiceCollection();

            // Act
            configurator.WithEventSerializer(serializer.Object);
            configurator.WithTypeRegistry(typeRegistry.Object);
            configurator.WithSingleTenant(registrator =>
                {
                    registrator.RegisterDependencies(c =>
                    {
                        c.AddSingleton<IStreamDatabase>(database.Object);
                        c.AddSingleton<IStreamReader>(database.Object);
                    });
                });

            configurator.Configure(services);

            var provider = services.BuildServiceProvider();

            // Assert
            provider.GetRequiredService<IStreamStore>().Should().NotBeNull();

            provider.GetRequiredService<IStreamDatabase>()
                        .Should().NotBeNull()
                        .And.Be(database.Object);

            provider.GetRequiredService<IStreamReader>()
                        .Should().NotBeNull()
                        .And.Be(database.Object);

            provider.GetRequiredService<IEventSerializer>()
                        .Should().NotBeNull()
                        .And.Be(serializer.Object);

            provider.GetRequiredService<ITypeRegistry>()
                        .Should().NotBeNull()
                        .And.Be(typeRegistry.Object);

            var configuration = provider.GetRequiredService<StreamStoreConfiguration>();

            configuration.Should().NotBeNull();
        }

        [Fact]
        public void When_database_backend_is_not_configured_manually()
        {
            // Arrange
            var configurator = StreamStoreConfiguratorSuite.CreateConfigurator();

            configurator.WithSingleTenant((x) => { });

            // Act
            var act = () => configurator.Configure(StreamStoreConfiguratorSuite.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend (IStreamDatabase) is not registered");

            // Arrange
            configurator.WithSingleTenant((x) =>
            {
                x.RegisterDependencies(c => c.AddSingleton<IStreamDatabase>(Generated.MockOf<IStreamDatabase>().Object));
            });

            // Act
            act = () => configurator.Configure(StreamStoreConfiguratorSuite.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend (IStreamReader) is not registered");
        }
    }
}
