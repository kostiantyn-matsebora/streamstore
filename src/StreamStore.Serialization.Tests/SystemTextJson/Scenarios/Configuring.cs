using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.InMemory;

namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public class Configuring
    {
        [Fact]
        public void When_configuring_use_system_text_json()
        {

            // Arrange
            var configurator = new SerializationConfigurator();

            // Act
            var collection = configurator.UseTextJsonSerializer().Configure();

            // Assert
            collection.Should().ContainSingle(x => x.ServiceType == typeof(IEventSerializer) && x.ImplementationType == typeof(SystemTextJsonEventSerializer));

        }
    }
}
