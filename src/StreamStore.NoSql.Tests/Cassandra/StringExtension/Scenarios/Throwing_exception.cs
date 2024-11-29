using FluentAssertions;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.StringExtension
{
    public class Throwing_exception: Scenario
    {
        [Fact]
        public void When_string_is_null_or_empty()
        {
            // Arrange
            string? value = null;

            // Act
            var act = () => value.ThrowIfNullOrEmpty("value");

            // Assert
            act.Should().Throw<ArgumentNullException>();



            // Arrange
            value = string.Empty;


            // Act
            act = () => value.ThrowIfNullOrEmpty("value");

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
