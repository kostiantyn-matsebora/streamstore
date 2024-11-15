using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.Object
{
    public class Loading_object : Scenario<S3StorageTestSuite>
    {

        [Fact]
        public async Task When_object_is_not_found()
        {
            // Arrange
            var s3Object = Suite.CreateS3Object();
            CancellationToken token = default;
            Suite.MockS3Client.Setup(x => x.FindObjectAsync(Suite.Path, token)).ReturnsAsync((FindObjectResponse?)null);
            Suite.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            await s3Object.LoadAsync(token);

            // Assert
            s3Object.State.Should().Be(S3ObjectState.DoesNotExist);
            s3Object.Data.Should().BeNull();
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_object_exists()
        {
            // Arrange
            var s3Object = Suite.CreateS3Object();
            var fixture = new Fixture();
            var response = fixture.Create<FindObjectResponse>();

            CancellationToken token = default;
            Suite.MockS3Client.Setup(x => x.FindObjectAsync(Suite.Path, token)).ReturnsAsync(response);
            Suite.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            await s3Object.LoadAsync(token);

            // Assert
            s3Object.State.Should().Be(S3ObjectState.Loaded);
            s3Object.Data.Should().NotBeNull();
            s3Object.Data.Should().BeSameAs(response.Data); 
            Suite.MockRepository.VerifyAll();
        }
    }
}
