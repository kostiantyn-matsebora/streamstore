using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class Disposing_client: Scenario<AWSS3ClientTestEnvironment>
    {
        [Fact]
        public async Task When_disposing_client()
        {
            // Arrange
            var client = Environment.Client;

            Environment.AmazonClient.Setup(m => m.Dispose());
            // Act
            await client.DisposeAsync();

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
