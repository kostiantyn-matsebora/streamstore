using Bytewizer.Backblaze.Client;
using FluentAssertions;
using StreamStore.S3.B2;

namespace StreamStore.S3.Tests.B2
{
    public class BackblazeClientFactoryTests
    {
        [Fact]
        public void Create_Should_CreateBackblazeClient()
        {
            // Arrange
            var factory = new BackblazeClientFactory();

            // Act
            var result = factory.Create();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BackblazeClient>();

        }
    }
}
