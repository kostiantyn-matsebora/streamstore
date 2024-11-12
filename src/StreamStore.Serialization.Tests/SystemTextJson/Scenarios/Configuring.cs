using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;

namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public class Configuring
    {
        [Fact]
        public void When_configuring_use_system_text_json()
        {

            // Arrange
            var configurator = new StreamStoreConfigurator();
            var collection = new ServiceCollection();

            // Act
            configurator.WithTextJsonSerializer();
            configurator.WithSingleTenant(x => x.UseInMemoryDatabase());
            configurator.Configure(collection);

            // Assert
            collection.Should().ContainSingle(x => x.ServiceType == typeof(IEventSerializer) && x.ImplementationType == typeof(SystemTextJsonEventSerializer));

        }
    }
}
