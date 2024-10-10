using Bytewizer.Backblaze.Client;
using FluentAssertions;
using Moq;
using StreamStore.S3.B2;

namespace StreamStore.S3.Tests.B2
{
    public class BackblazeClientFactoryTests
    {
        BackblazeClientFactory CreateFactory()
        {
            return new BackblazeClientFactory();
        }

        [Fact]
        public void Create_Should_CreateBackblazeClient()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result = factory.Create();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BackblazeClient>();

        }
    }
}
