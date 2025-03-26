using AutoFixture;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.Object
{
    public class Uploading_object: Scenario<S3StorageTestSuite>
    {
        [Fact]
        public async Task When_uploading_object()
        {
            // Arrange
            var s3Object = Suite.CreateS3Object();
            var fixture = new Fixture();

            var response = new UploadObjectResponse
            {
                Key = s3Object.Path,
                VersionId = Generated.Primitives.String
            };

            CancellationToken token = default;
            Suite.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));
            Suite.MockS3Client.Setup(x => x.UploadObjectAsync(It.Is<UploadObjectRequest>(r => r.Key == s3Object.Path && r.Data == s3Object.Data), token)).ReturnsAsync(response);

            // Act
            await s3Object.UploadAsync(token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }

    }
}
