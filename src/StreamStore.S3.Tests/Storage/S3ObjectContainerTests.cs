using AutoFixture;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage
{
    public class S3ObjectContainerTests
    {
        readonly MockRepository mockRepository;
        readonly Mock<IS3Client> mockS3Client;
        readonly Mock<IS3ClientFactory> mockS3ClientFactory;
        readonly S3ContainerPath path;

        public S3ObjectContainerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockS3Client = new Mock<IS3Client>(MockBehavior.Strict);
            this.mockS3ClientFactory = this.mockRepository.Create<IS3ClientFactory>();
            this.path = new S3ContainerPath(Generated.String);

            mockS3ClientFactory
                .Setup(x => x.CreateClient())
                .Returns(mockS3Client.Object);
        }

        S3ObjectContainer CreateS3ObjectContainer()
        {
            return new S3ObjectContainer(path, mockS3ClientFactory.Object);
        }

        [Fact]
        public async Task DeleteAsync_Should_CallDeletionOnClient()
        {
            // Arrange
            var s3ObjectContainer = this.CreateS3ObjectContainer();
            CancellationToken token = default;
            var fixture = new Fixture();
            var response = fixture.Create<ListS3ObjectsResponse>();
            var lastObject = response.Objects!.Last();
            var nextResponse = fixture.Create<ListS3ObjectsResponse>();
            nextResponse.Objects = Enumerable.Empty<ObjectDescriptor>().ToArray();
            
            mockS3Client.SetupSequence(x => x.ListObjectsAsync(path.Normalize(), null, token)).ReturnsAsync(response);
            mockS3Client.Setup(x => x.ListObjectsAsync(path.Normalize(), lastObject.FileName, token)).ReturnsAsync(nextResponse);
            mockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            foreach (var obj in response.Objects!)
            {
                mockS3Client.Setup(x => x.DeleteObjectByFileIdAsync(obj.FileId!, obj.FileName!, token)).Returns(Task.CompletedTask);
            }

            // Act
            await s3ObjectContainer.DeleteAsync(token);

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
