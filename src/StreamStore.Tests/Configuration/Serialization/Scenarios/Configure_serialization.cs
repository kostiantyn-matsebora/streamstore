using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;
using StreamStore.Testing;


namespace StreamStore.Tests.Configuration.Serialization { 
    public class Configure_serialization: Scenario<SerializationTestEnvironment>
    {

        [Fact]
        public void When_configured_with_default_values()
        {
            // Arrange 
            var configurator = SerializationTestEnvironment.CreateConfigurator();

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
            var configurator = SerializationTestEnvironment.CreateConfigurator();

            // Act
            var services = configurator
                            .UseTextJsonSerializer()
                            .UseTypeRegistry<SerializationTestEnvironment.FakeTypeRegistry>()
                            .EnableCompression()
                            .Configure();

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IEventSerializer>()
                    .Should().NotBeNull()
                    .And.BeOfType<SystemTextJsonEventSerializer>();

            provider.GetRequiredService<ITypeRegistry>()
                    .Should().NotBeNull()
                    .And.BeOfType<SerializationTestEnvironment.FakeTypeRegistry>();

            var configuration = provider.GetRequiredService<SerializationConfiguration>();

            configuration.Should().NotBeNull();
            configuration.CompressionEnabled.Should().BeTrue();
        }
    }
}
