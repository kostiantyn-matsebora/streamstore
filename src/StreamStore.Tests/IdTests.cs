using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests
{
    public class IdTests
    {
        static Id CreateId(string? id = null)
        {
            return new Id(id ?? Generated.String);
        }

        [Fact]
        public void Equals_Should_BeEquivalentStringValue()
        {
            // Arrange
            var expected = Generated.String;
            var id = CreateId(expected);

            
            // Assert
            id.Equals(expected).Should().BeTrue();
            id.Equals(new Id(expected)).Should().BeTrue(); 
            id.Equals(Generated.String).Should().BeFalse();
            id.Equals(CreateId()).Should().BeFalse();
            id.Equals(string.Empty).Should().BeFalse();
            id.Equals(new object()).Should().BeFalse();
            id.Equals((object)expected).Should().BeTrue();
            id.Equals((object)Generated.String).Should().BeFalse();
        }
     

        [Fact]
        public void GetHashCode_Should_ReturnStringValueHashCode()
        {
            // Arrange
            var expected = Generated.String;
            var id = CreateId(expected);

            // Act
            var result = id.GetHashCode();

            // Assert
            result.Should().Be(expected.GetHashCode());
        }

        [Fact]
        public void ToString_Should_ReturnStringValue()
        {
            // Arrange
            var expected = Generated.String;
            var id = CreateId(expected);

            // Act
            var result = id.ToString();

            // Assert
            result.Should().Be(expected);
            result.Should().Be(expected.ToString());
        }

        [Fact]
        public void Operator_Should_CompareStringValue()
        {
            var expected = Generated.String;

            // Arrange
            var id = CreateId(expected);

            // Assert
            (id == expected).Should().BeTrue();
            (id == new Id(expected)).Should().BeTrue();
            (id != Generated.String).Should().BeTrue();
            (id == Generated.String).Should().BeFalse();
            (id != CreateId()).Should().BeTrue();
        }
    }
}
