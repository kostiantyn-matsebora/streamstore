using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;
using StreamStore.Testing;
using static StreamStore.Tests.Configuration.Serialization.SerializationTestSuite;

namespace StreamStore.Tests.Configuration.Serialization { 
    public class Configure_serialization: Scenario<SerializationTestSuite>
    {

        [Fact]
        public void When_configured_with_default_values()
        {
            // Arrange 
            var configurator = CreateConfigurator();

            // Act
            var services = configurator.Configure();

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IEventSerializer>().Should().NotBeNull().And.BeOfType<NewtonsoftEventSerializer>();
            provider.GetRequiredService<ITypeRegistry>().Should().NotBeNull().And.BeOfType<TypeRegistry>();
            var configuration = provider.GetRequiredService<SerializationConfiguration>();

            configuration.Should().NotBeNull();
            configuration.CompressionEnabled.Should().BeFalse();
        }


        [Fact]
        public void When_configured_with_custom_dependencies()
        {
            // Arrange 
            var configurator = CreateConfigurator();

            // Act
            var services = configurator
                            .UseTextJsonSerializer()
                            .UseTypeRegistry<FakeTypeRegistry>()
                            .EnableCompression()
                            .Configure();

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IEventSerializer>().Should().NotBeNull().And.BeOfType<SystemTextJsonEventSerializer>();
            provider.GetRequiredService<ITypeRegistry>().Should().NotBeNull().And.BeOfType<FakeTypeRegistry>();
            var configuration = provider.GetRequiredService<SerializationConfiguration>();

            configuration.Should().NotBeNull();
            configuration.CompressionEnabled.Should().BeTrue();
        }
    }
}
