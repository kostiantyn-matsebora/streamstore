using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using Moq;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Copying_objects: Scenario<B2S3ClientSuite>
    {

        [Fact]
        public async Task When_copy_object_by_id()
        {
            // Arrange
            var client = Suite.CreateB2S3Client();
            string sourceFileId = Generated.Primitives.String;
            string sourceName = Generated.Primitives.String;
            string destinationName = Generated.Primitives.String;
            var files = new Mock<IStorageFiles>();
            Suite.B2Client.SetupGet(m => m.Files).Returns(files.Object);

            CancellationToken token = default;

            files.Setup(m => m.CopyAsync(It.IsAny<CopyFileRequest>()));

            // Act
            await client.CopyByVersionIdAsync(
                sourceFileId,
                sourceName,
                destinationName,
                token);

            // Assert
            Suite.MockRepository.VerifyAll();
            files.VerifyAll();
        }
    }
}
