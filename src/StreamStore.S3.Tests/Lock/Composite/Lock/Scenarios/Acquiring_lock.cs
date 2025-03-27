using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Tests.Lock.Composite.LockHandle;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Lock.Composite.Lock
{
    public class Acquiring_lock: Scenario<S3CompositeLockTestEnvironment>
    {
        [Fact]
        public async Task When_acquiring_with_multiple_internal_locks()
        {
            // Arrange
            var s3CompositeStreamLock = Environment.CreateLock();
            CancellationToken token = default;

            Environment.Lock1.Setup(m =>
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IS3LockHandle>().Object);
            Environment.Lock2.Setup(m =>
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IS3LockHandle>().Object);

            // Act
            var result = await s3CompositeStreamLock.AcquireAsync(token);

            // Assert
            Environment.MockRepository.VerifyAll();
        }
        [Fact]
        public async Task When_one_of_the_internal_locks_is_not_acquired()
        {

            // Arrange
            var s3CompositeStreamLock = Environment.CreateLock();
            CancellationToken token = default;
            var handle1 = new Mock<IS3LockHandle>();

            Environment.Lock1.Setup(m =>
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(handle1.Object);

            Environment.Lock2.Setup(m =>
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IS3LockHandle?)null);

            handle1.Setup(m => m.ReleaseAsync(It.IsAny<CancellationToken>()));

            // Act
            var result = await s3CompositeStreamLock.AcquireAsync(token);

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
