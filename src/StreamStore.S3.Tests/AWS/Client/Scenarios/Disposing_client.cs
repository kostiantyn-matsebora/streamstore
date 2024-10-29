using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class Disposing_client: Scenario<AWSS3ClientSuite>
    {
        [Fact]
        public async Task When_disposing_client()
        {
            // Arrange
            var client = Suite.Client;

            Suite.AmazonClient.Setup(m => m.Dispose());
            // Act
            await client.DisposeAsync();

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
