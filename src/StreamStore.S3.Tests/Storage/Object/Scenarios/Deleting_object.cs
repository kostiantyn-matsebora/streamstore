
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.Object { 
    public class Deleting_object: Scenario<S3StorageTestEnvironment>
    {

        [Fact]
        public async Task When_object_does_not_exist()
        {
            // Arrange
            var s3Object = Environment.CreateS3Object();
            CancellationToken token = default;
            Environment.MockS3Client.Setup(x => x.FindObjectDescriptorAsync(Environment.Path, token)).ReturnsAsync((ObjectDescriptor?)null);
            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            await s3Object.DeleteAsync(token);

            // Assert
            s3Object.State.Should().Be(S3ObjectState.Unknown);
            s3Object.Data.Should().BeNull();
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_object_exists()
        {
            // Arrange
            var s3Object = Environment.CreateS3Object();
            var key = Generated.Primitives.String;
            var versionId = Generated.Primitives.String;

            CancellationToken token = default;
            Environment.MockS3Client.SetupSequence(x => x.FindObjectDescriptorAsync(Environment.Path, token))
                              .ReturnsAsync((ObjectDescriptor?)new ObjectDescriptor { Key = key, VersionId = versionId })
                              .ReturnsAsync((ObjectDescriptor?)null);
            Environment.MockS3Client.Setup(x => x.DeleteObjectByVersionIdAsync(versionId, key, token)).Returns(Task.CompletedTask);

            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            await s3Object.DeleteAsync(token);

            // Assert
            s3Object.State.Should().Be(S3ObjectState.Unknown);
            s3Object.Data.Should().BeNull();
            Environment.MockRepository.VerifyAll();
        }
    }
}
