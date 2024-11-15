using Bytewizer.Backblaze.Client;
using FluentAssertions;
using StreamStore.S3.B2;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.B2ClientFactory
{
    public class Creating_client: Scenario
    {
        [Fact]
        public void When_creating_client()
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
