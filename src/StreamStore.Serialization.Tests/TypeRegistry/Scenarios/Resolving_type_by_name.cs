using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Serialization.Tests.TypeRegistry
{
    public class Resolving_type_by_name : Scenario<TypeRegistryTestEnvironment>
    {
        [Fact]
        public void When_type_loaded_to_domain()
        {
            // Act
            var type = Environment.TypeRegistry.ResolveTypeByName("StreamStore.Serialization.Tests.TypeRegistry.TypeRegistryTestEnvironment, StreamStore.Serialization.Tests");

            // Assert
            type.Should().Be(typeof(TypeRegistryTestEnvironment));
        }
    }
}
