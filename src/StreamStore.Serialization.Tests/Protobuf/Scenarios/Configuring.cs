using StreamStore.Testing;
using StreamStore.Serialization.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using FluentAssertions;
using StreamStore.Configuration;

namespace StreamStore.Serialization.Tests.Protobuf
{
    public class Configuring: Scenario
    {

        [Fact]
        public void When_configuring_use_protobuf()
        {

            // Arrange
            var configurator = new SerializationConfigurator();

            // Act
           var collection = configurator.WithProtobufSerializer(true).Configure();
         

            // Assert
            collection.Should().ContainSingle(x => x.ServiceType == typeof(IEventSerializer) && x.ImplementationType == typeof(ProtobufEventSerializer));

        }
    }
}
