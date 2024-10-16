using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Testing;

namespace StreamStore.Tests
{
    public class PessimisticConcurrencyExceptionTests
    {
        [Fact]
        public void Should_SetStreamId()
        {

            // Arrange
            var streamId = new Id(GeneratedValues.String);

            // Act
            var exception = new StreamLockedException(streamId);

            // Assert
            exception.Should().NotBeNull();
            exception.StreamId.Should().Be(streamId);
        }
    }
}
