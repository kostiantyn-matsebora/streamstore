using Bytewizer.Backblaze.Client;
using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Deleting_objects: Scenario<B2S3ClientTestEnvironment>
    {

        [Fact]
        public async Task When_delete_object_by_id()
        {
            // Arrange
            var client = Environment.CreateB2S3Client();
            string fileId = Generated.Primitives.String;
            string key = Generated.Primitives.String;
            string destinationName = Generated.Primitives.String;
            var files = new Mock<IStorageFiles>();
            Environment.B2Client.SetupGet(m => m.Files).Returns(files.Object);


            CancellationToken token = default;

            files.Setup(m => m.DeleteAsync(fileId, key));

            // Act
            await client.DeleteObjectByVersionIdAsync(fileId, key, token);

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
