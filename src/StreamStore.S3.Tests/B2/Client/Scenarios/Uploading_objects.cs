using Bytewizer.Backblaze.Models;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Client
{
    public class Uploading_objects: Scenario<B2S3ClientTestEnvironment>
    {
        [Fact]
        public async Task When_uploading_object()
        {
            // Arrange
            var aWSS3Client = Environment.CreateB2S3Client();
            string key = Generated.Primitives.String;
            string fileId = Generated.Primitives.String;
            CancellationToken token = default;
            var content = Generated.Objects.ByteArray;
            var request = new UploadObjectRequest
            {
                Key = key,
                Data = content
            };

            var apiResults = Environment.MockRepository.Create<IApiResults<UploadFileResponse>>();
            apiResults.SetupGet(m => m.Response).Returns(new UploadFileResponse() { FileName = key, FileId = fileId });
            apiResults.SetupGet(m => m.IsSuccessStatusCode).Returns(true);

            Environment.B2Client
                .Setup(m => m.UploadAsync(Environment.Settings.BucketId, key, It.IsAny<System.IO.Stream>()))
                .ReturnsAsync(apiResults.Object);

            // Act
            var result = await aWSS3Client.UploadObjectAsync(request, token);

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
