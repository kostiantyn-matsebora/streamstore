using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.Tests.Lock
{
    public class S3CompositeStreamLockTests
    {
        readonly MockRepository mockRepository;
        readonly Mock<IS3StreamLock> lock1;
        readonly Mock<IS3StreamLock> lock2;


        public S3CompositeStreamLockTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.lock1 = this.mockRepository.Create<IS3StreamLock>();
            this.lock2 = this.mockRepository.Create<IS3StreamLock>();
        }

        S3CompositeStreamLock CreateS3CompositeStreamLock()
        {
            return new S3CompositeStreamLock(lock1.Object, lock2.Object);
        }

        [Fact]
        public async Task AcquireAsync_ShoudAcquireBothLocks()
        {
            // Arrange
            var s3CompositeStreamLock = CreateS3CompositeStreamLock();
            CancellationToken token = default;

            lock1.Setup(m => 
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IS3LockHandle>().Object);
            lock2.Setup(m => 
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IS3LockHandle>().Object);

            // Act
            var result = await s3CompositeStreamLock.AcquireAsync(token);

            // Assert
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task AcquireAsync_ShoudReleaseLockIfNextIsNotAcquired()
        {

            // Arrange
            var s3CompositeStreamLock = CreateS3CompositeStreamLock();
            CancellationToken token = default;
            var handle1 = new Mock<IS3LockHandle>();
            
            lock1.Setup(m => 
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(handle1.Object);
            
            lock2.Setup(m => 
               m.AcquireAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IS3LockHandle?)null);

            handle1.Setup(m => m.ReleaseAsync(It.IsAny<CancellationToken>()));
               
            // Act
            var result = await s3CompositeStreamLock.AcquireAsync(token);

            // Assert
            this.mockRepository.VerifyAll();
        }
    }
}
