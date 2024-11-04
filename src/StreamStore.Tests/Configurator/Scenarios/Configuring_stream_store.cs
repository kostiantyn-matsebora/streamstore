

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
            var configurator = Suite.CreateConfigurator();

            // Act
            var act = () => configurator.Configure(Suite.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend is not set");
        }

        [Fact]
        public void When_store_configured_by_types()
        {
            // Arrange
            var database = Generated.MockOf<IStreamDatabase>();
            var typeRegistry = Generated.MockOf<ITypeRegistry>();
            var configurator = Suite.CreateConfigurator();
            var pageSize = Generated.Int;
            var mode = StreamReadingMode.ProduceConsume;
            var services = Suite.CreateServiceCollection();

            // Act
            configurator.WithReadingPageSize(pageSize);
            configurator.WithReadingMode(mode);
            configurator.WithCompression();
            configurator.WithEventSerializer<SystemTextJsonEventSerializer>();
            configurator.WithTypeRegistry<TypeRegistry>();
            configurator.WithDatabase<InMemoryStreamDatabase>();

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
            configuration.Compression.Should().BeTrue();
        }

        [Fact]
        public void When_store_configured_by_instances()
        {
            // Arrange
            var database = Generated.MockOf<IStreamDatabase>();
            var typeRegistry = Generated.MockOf<ITypeRegistry>();
            var serializer = Generated.MockOf<IEventSerializer>();
            var configurator = Suite.CreateConfigurator();
            var services = Suite.CreateServiceCollection();

            // Act
            configurator.WithEventSerializer(serializer.Object);
            configurator.WithTypeRegistry(typeRegistry.Object);
            configurator.WithDatabase(c =>
                {
                    c.AddSingleton<IStreamDatabase>(database.Object);
                    c.AddSingleton<IStreamReader>(database.Object);
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
            var configurator = Suite.CreateConfigurator();

            configurator.WithDatabase(x => { });

            // Act
            var act = () => configurator.Configure(Suite.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend (IStreamDatabase) is not registered");

            // Arrange
            configurator.WithDatabase(c =>
            {
                c.AddSingleton<IStreamDatabase>(Generated.MockOf<IStreamDatabase>().Object);
            });

            // Act
            act = () => configurator.Configure(Suite.CreateServiceCollection());

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Database backend (IStreamReader) is not registered");
        }
    }
}
