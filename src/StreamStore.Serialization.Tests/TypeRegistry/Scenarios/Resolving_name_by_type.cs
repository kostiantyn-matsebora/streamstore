using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Serialization.Tests.TypeRegistry
{
    public class Resolving_name_by_type : Scenario<TypeRegistryTestEnvironment>
    {
        [Fact]
        public void When_type_loaded_to_domain()
        {
            // Act
            var name = Environment.TypeRegistry.ResolveNameByType(typeof(TypeRegistryTestEnvironment));

            // Assert
            name.Should().Be("StreamStore.Serialization.Tests.TypeRegistry.TypeRegistryTestEnvironment, StreamStore.Serialization.Tests");
        }
    }
}
