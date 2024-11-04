using Bytewizer.Backblaze.Client;
using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Deleting_objects: Scenario<B2S3ClientSuite>
    {

        [Fact]
        public async Task When_delete_object_by_id()
        {
            // Arrange
            var client = Suite.CreateB2S3Client();
            string fileId = Generated.String;
            string key = Generated.String;
            string destinationName = Generated.String;
            var files = new Mock<IStorageFiles>();
            Suite.B2Client.SetupGet(m => m.Files).Returns(files.Object);


            CancellationToken token = default;

            files.Setup(m => m.DeleteAsync(fileId, key));

            // Act
            await client.DeleteObjectByVersionIdAsync(fileId, key, token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
