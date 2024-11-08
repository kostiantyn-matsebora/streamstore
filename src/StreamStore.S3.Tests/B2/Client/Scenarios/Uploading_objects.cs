using Bytewizer.Backblaze.Models;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Uploading_objects: Scenario<B2S3ClientSuite>
    {
        [Fact]
        public async Task When_uploading_object()
        {
            // Arrange
            var aWSS3Client = Suite.CreateB2S3Client();
            string key = Generated.String;
            string fileId = Generated.String;
            CancellationToken token = default;
            var content = Generated.ByteArray;
            var request = new UploadObjectRequest
            {
                Key = key,
                Data = content
            };

            var apiResults = Suite.MockRepository.Create<IApiResults<UploadFileResponse>>();
            apiResults.SetupGet(m => m.Response).Returns(new UploadFileResponse() { FileName = key, FileId = fileId });
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);

            Suite.B2Client
                .Setup(m => m.UploadAsync(Suite.Settings.BucketId, key, It.IsAny<System.IO.Stream>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await aWSS3Client.UploadObjectAsync(request, token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
