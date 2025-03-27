using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.Object
{
    public class Loading_object : Scenario<S3StorageTestEnvironment>
    {

        [Fact]
        public async Task When_object_is_not_found()
        {
            // Arrange
            var s3Object = Environment.CreateS3Object();
            CancellationToken token = default;
            Environment.MockS3Client.Setup(x => x.FindObjectAsync(Environment.Path, token)).ReturnsAsync((FindObjectResponse?)null);
            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            await s3Object.LoadAsync(token);

            // Assert
            s3Object.State.Should().Be(S3ObjectState.DoesNotExist);
            s3Object.Data.Should().BeNull();
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_object_exists()
        {
            // Arrange
            var s3Object = Environment.CreateS3Object();
            var fixture = new Fixture();
            var response = fixture.Create<FindObjectResponse>();

            CancellationToken token = default;
            Environment.MockS3Client.Setup(x => x.FindObjectAsync(Environment.Path, token)).ReturnsAsync(response);
            Environment.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            // Act
            await s3Object.LoadAsync(token);

            // Assert
            s3Object.State.Should().Be(S3ObjectState.Loaded);
            s3Object.Data.Should().NotBeNull();
            s3Object.Data.Should().BeSameAs(response.Data); 
            Environment.MockRepository.VerifyAll();
        }
    }
}
