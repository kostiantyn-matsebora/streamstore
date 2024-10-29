using Amazon.S3.Model;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class Uploading_objects: Scenario<AWSS3ClientSuite>
    {

        [Fact]
        public async Task When_uploading_object()
        {
            // Arrange
            var aWSS3Client = Suite.Client;
            string key = Generated.String;
            CancellationToken token = default;
            var content = Generated.ByteArray;
            var request = new UploadObjectRequest
            {
                Key = key,
                Data = content
            };

            Suite.AmazonClient.Setup(m => m.PutObjectAsync(
                It.Is<PutObjectRequest>(r =>
                    r.BucketName == Suite.Settings.BucketName
                    && r.Key == key
                    && r.InputStream != null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PutObjectResponse());

            // Act
            var result = await aWSS3Client.UploadObjectAsync(request, token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
