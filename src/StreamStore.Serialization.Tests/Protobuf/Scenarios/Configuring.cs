using StreamStore.Testing;
using StreamStore.Serialization.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using FluentAssertions;

namespace StreamStore.Serialization.Tests.Protobuf
{
    public class Configuring: Scenario
    {

        [Fact]
        public void When_configuring_use_protobuf()
        {

            // Arrange
            var configurator = new StreamStoreConfigurator();
            var collection = new ServiceCollection();

            // Act
            configurator.WithProtobufSerializer(true);
            configurator.UseInMemoryDatabase();
            configurator.Configure(collection);

            // Assert
            collection.Should().ContainSingle(x => x.ServiceType == typeof(IEventSerializer) && x.ImplementationType == typeof(ProtobufEventSerializer));

        }
    }
}
