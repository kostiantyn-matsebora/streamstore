using AutoFixture;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Lock;
using StreamStore.S3.Storage;
using StreamStore.Testing;


namespace StreamStore.S3.Tests.Lock
{
    public class S3FileLockHandleTests
    {
        readonly MockRepository mockRepository;

        readonly Mock<IS3ClientFactory> mockClientFactory;
        readonly S3FileLockHandle s3FileLockHandle;
        readonly S3Object lockObject;

        public S3FileLockHandleTests()
        {
            var fixture = new Fixture();
            this.mockRepository = new MockRepository(MockBehavior.Strict);
           
            this.mockClientFactory = this.mockRepository.Create<IS3ClientFactory>();

            var client = new Mock<IS3Client>();
            mockClientFactory.Setup(x => x.CreateClient()).Returns(client.Object);
            lockObject = new S3Object(new S3ContainerPath(GeneratedValues.String), mockClientFactory.Object);
            this.s3FileLockHandle = this.CreateS3FileLockHandle();
        }

        S3FileLockHandle CreateS3FileLockHandle()
        {
            return new S3FileLockHandle(lockObject);
        }

        [Fact]
        public async Task ReleaseAsync_ShouldDeleteLock()
        {
            // Arrange
            CancellationToken token = default;


            // Act
            await s3FileLockHandle.ReleaseAsync(token);
            await s3FileLockHandle.ReleaseAsync(token); //Checking that the lock is only released once

            // Assert
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DisposeAsync_Should_ReleaseLock()
        {
            // Act
            await s3FileLockHandle.DisposeAsync();
            await s3FileLockHandle.DisposeAsync(); //Checking that the lock is only released once

            // Assert
            this.mockRepository.VerifyAll();
        }
    }
}
