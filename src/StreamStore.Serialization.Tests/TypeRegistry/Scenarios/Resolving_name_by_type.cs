using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Serialization.Tests.TypeRegistry
{
    public class Resolving_name_by_type : Scenario<TypeRegistrySuite>
    {
        [Fact]
        public void When_type_loaded_to_domain()
        {
            // Act
            var name = Suite.TypeRegistry.ResolveNameByType(typeof(TypeRegistrySuite));

            // Assert
            name.Should().Be("StreamStore.Serialization.Tests.TypeRegistry.TypeRegistrySuite, StreamStore.Serialization.Tests");
        }
    }
}
