using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.Tests
{
    public class IdTests
    {
        Id CreateId(string? id = null)
        {
            return new Id(id ?? GeneratedValues.String);
        }

        [Fact]
        public void Equals_Should_BeEquivalentStringValue()
        {
            // Arrange
            var expected = GeneratedValues.String;
            var id = this.CreateId(expected);

            
            // Assert
            id.Equals(expected).Should().BeTrue();
            id.Equals(new Id(expected)).Should().BeTrue(); 
            id.Equals(GeneratedValues.String).Should().BeFalse();
            id.Equals(CreateId()).Should().BeFalse();
            id.Equals((string)null).Should().BeFalse();
            id.Equals(new object()).Should().BeFalse();
            id.Equals((object)expected).Should().BeTrue();
            id.Equals((object)GeneratedValues.String).Should().BeFalse();
        }
     

        [Fact]
        public void GetHashCode_Should_ReturnStringValueHashCode()
        {
            // Arrange
            var expected = GeneratedValues.String;
            var id = this.CreateId(expected);

            // Act
            var result = id.GetHashCode();

            // Assert
            result.Should().Be(expected.GetHashCode());
        }

        [Fact]
        public void ToString_Should_ReturnStringValue()
        {
            // Arrange
            var expected = GeneratedValues.String;
            var id = this.CreateId(expected);

            // Act
            var result = id.ToString();

            // Assert
            result.Should().Be(expected);
            result.Should().Be(expected.ToString());
        }

        [Fact]
        public void Operator_Should_CompareStringValue()
        {
            var expected = GeneratedValues.String;

            // Arrange
            var id = this.CreateId(expected);

            // Assert
            (id == expected).Should().BeTrue();
            (id == new Id(expected)).Should().BeTrue();
            (id != GeneratedValues.String).Should().BeTrue();
            (id == GeneratedValues.String).Should().BeFalse();
            (id != CreateId()).Should().BeTrue();
        }
    }
}
