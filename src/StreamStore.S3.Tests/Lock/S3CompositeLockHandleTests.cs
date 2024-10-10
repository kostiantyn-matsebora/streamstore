using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;


namespace StreamStore.S3.Tests.Lock
{
    public class S3CompositeLockHandleTests
    {
        Mock<IS3LockHandle> firstHandle;
        Mock<IS3LockHandle> secondHandle;


        public S3CompositeLockHandleTests()
        {
            firstHandle = new Mock<IS3LockHandle>();
            secondHandle = new Mock<IS3LockHandle>();
        }

        S3CompositeLockHandle CreateHandle()
        {
            return new S3CompositeLockHandle(firstHandle.Object, secondHandle.Object);
        }

        [Fact]
        public async Task DisposeAsync_Should_DisposeAllHandles()
        {
            // Arrange
            var handle = this.CreateHandle();

            // Act
            await handle.DisposeAsync();

            // Assert
            firstHandle.Verify(m => m.DisposeAsync(), Times.Once);
            secondHandle.Verify(m => m.DisposeAsync(), Times.Once);
        }

        [Fact]
        public async Task ReleaseAsync_ShouldReleaseAllLocks()
        {
            // Arrange
            var handle = this.CreateHandle();
            CancellationToken token = default(global::System.Threading.CancellationToken);

            // Act
            await handle.ReleaseAsync(
                token);

            // Assert
            firstHandle.Verify(m => m.ReleaseAsync(It.IsAny<CancellationToken>()), Times.Once);
            secondHandle.Verify(m => m.ReleaseAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
