using Amazon.S3.Model;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class Uploading_objects: Scenario<AWSS3ClientTestEnvironment>
    {

        [Fact]
        public async Task When_uploading_object()
        {
            // Arrange
            var aWSS3Client = Environment.Client;
            string key = Generated.Primitives.String;
            CancellationToken token = default;
            var content = Generated.Objects.ByteArray;
            var request = new UploadObjectRequest
            {
                Key = key,
                Data = content
            };

            Environment.AmazonClient.Setup(m => m.PutObjectAsync(
                It.Is<PutObjectRequest>(r =>
                    r.BucketName == Environment.Settings.BucketName
                    && r.Key == key
                    && r.InputStream != null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PutObjectResponse());

            // Act
            var result = await aWSS3Client.UploadObjectAsync(request, token);

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
