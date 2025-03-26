using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests.Identifier
{
    public class When_overriding_object : Scenario<IdentifierTestSuite>
    {
        public When_overriding_object() : base(new IdentifierTestSuite())
        {
        }

        [Fact]
        public void When_gettings_hashcode()
        {
            // Arrange
            var expected = Generated.Primitives.String;
            var id = IdentifierTestSuite.CreateId(expected);

            // Act
            var result = id.GetHashCode();

            // Assert
            result.Should().Be(expected.GetHashCode());
        }

        [Fact]
        public void When_using_to_string()
        {
            // Arrange
            var expected = Generated.Primitives.String;
            var id = IdentifierTestSuite.CreateId(expected);

            // Act
            var result = id.ToString();

            // Assert
            result.Should().Be(expected);
            result.Should().Be(expected.ToString());
        }
    }
}
