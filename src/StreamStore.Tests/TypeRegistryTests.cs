using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.Serialization;

namespace StreamStore.Tests
{
    public class TypeRegistryTests
    {

        readonly TypeRegistry registry;


        public TypeRegistryTests()
        {
            registry = TypeRegistry.CreateAndInitialize();
        }


        [Fact]
        public void ByType_ShouldResolveIfTypeLoadedToDomain()
        {
           
            // Act
            var name = registry.ResolveNameByType(typeof(TypeRegistryTests));

            // Assert
            name.Should().Be("StreamStore.Tests.TypeRegistryTests, StreamStore.Tests");
        }

        [Fact]
        public void ByName_ShouldResolveIfTypeLoadedToDomain()
        {
            // Act
            var type = registry.ResolveTypeByName("StreamStore.Tests.TypeRegistryTests, StreamStore.Tests");

            // Assert
           type.Should().Be(typeof(TypeRegistryTests));
        }
    }
}
