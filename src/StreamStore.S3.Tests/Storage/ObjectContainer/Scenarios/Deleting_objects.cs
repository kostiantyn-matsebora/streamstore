using AutoFixture;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.ObjectContainer
{
    public class Deleting_objects: Scenario<S3StorageTestSuite>
    {

        [Fact]
        public async Task When_deleting_objects()
        {
            // Arrange
            var s3ObjectContainer = Suite.CreateS3ObjectContainer();
            CancellationToken token = default;
            var fixture = new Fixture();
            var response = fixture.Create<ListS3ObjectsResponse>();
            var lastObject = response.Objects!.Last();
            var nextResponse = fixture.Create<ListS3ObjectsResponse>();
            nextResponse.Objects = Enumerable.Empty<ObjectDescriptor>().ToArray();

            Suite.MockS3Client.SetupSequence(x => x.ListObjectsAsync(Suite.Path.Normalize(), null, token)).ReturnsAsync(response);
            Suite.MockS3Client.Setup(x => x.ListObjectsAsync(Suite.Path.Normalize(), lastObject.Key, token)).ReturnsAsync(nextResponse);
            Suite.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            foreach (var obj in response.Objects!)
            {
                Suite.MockS3Client.Setup(x => x.DeleteObjectByVersionIdAsync(obj.VersionId!, obj.Key!, token)).Returns(Task.CompletedTask);
            }

            // Act
            await s3ObjectContainer.DeleteAsync(token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
