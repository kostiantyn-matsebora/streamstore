using FluentAssertions;
using StreamStore.Testing;
using StreamStore.Tests.RevisionObject;

namespace StreamStore.Tests.Identifier
{
    public class When_comparing: Scenario<IdentifierTestEnvironment>
    {
        public When_comparing(): base(new IdentifierTestEnvironment())
        {
            
        }

        [Fact]
        public void When_comparing_different_type_values()
        {
            var expected = Generated.Primitives.String;

            // Arrange
            var id = IdentifierTestEnvironment.CreateId(expected);

            // Assert
            (id == expected).Should().BeTrue();
            (id == new Id(expected)).Should().BeTrue();
            (id != Generated.Primitives.String).Should().BeTrue();
            (id == Generated.Primitives.String).Should().BeFalse();
            (id != IdentifierTestEnvironment.CreateId()).Should().BeTrue();
        }

        [Fact]
        public void When_comparing_with_equality()
        {
            // Arrange
            var expected = Generated.Primitives.String;
            var id = IdentifierTestEnvironment.CreateId(expected);


            // Assert
            id.Equals(expected).Should().BeTrue();
            id.Equals(new Id(expected)).Should().BeTrue();
            id.Equals(Generated.Primitives.String).Should().BeFalse();
            id.Equals(IdentifierTestEnvironment.CreateId()).Should().BeFalse();
            id.Equals(string.Empty).Should().BeFalse();
            id.Equals(new object()).Should().BeFalse();
            id.Equals((object)expected).Should().BeTrue();
            id.Equals((object)Generated.Primitives.String).Should().BeFalse();
        }
    }
}
