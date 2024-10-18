//using Moq;
//using StreamStore.S3.Client;
//using StreamStore.S3.Concurrency;
//using StreamStore.S3.Lock;
//using StreamStore.Testing;


//namespace StreamStore.S3.Tests.Lock
//{
//    public class S3FileLockHandleTests
//    {
//        readonly MockRepository mockRepository;
//        readonly Mock<IS3TransactionContext> ctx;
//        readonly Mock<IS3LockFactory> mockFactory;
//        readonly S3FileLockHandle s3FileLockHandle;

//        public S3FileLockHandleTests()
//        {
//            this.mockRepository = new MockRepository(MockBehavior.Strict);
//            mockFactory = new Mock<IS3LockFactory>();
//            this.ctx = this.mockRepository.Create<IS3TransactionContext>();
//            this.mockFactory = this.mockRepository.Create<IS3LockFactory>();

//            var fileId = GeneratedValues.String;
//            this.s3FileLockHandle = this.CreateS3FileLockHandle(fileId);

//            CancellationToken token = default;
//            var client = new Mock<IS3Client>();
//            mockFactory.Setup(x => x.CreateClient()).Returns(client.Object);

//            var lockKey = GeneratedValues.String;
//            ctx.SetupGet(x => x.LockKey).Returns(lockKey);
//            client.Setup(x => x.DeleteObjectByFileIdAsync(fileId, lockKey, token)).Returns(Task.CompletedTask);
//        }

//        S3FileLockHandle CreateS3FileLockHandle(string fileId)
//        {
//            return new S3FileLockHandle(
//                this.ctx.Object,
//                fileId,
//                this.mockFactory.Object);
//        }

//        [Fact]
//        public async Task ReleaseAsync_ShouldDeleteLock()
//        {
//            // Arrange
//            CancellationToken token = default;


//            // Act
//            await s3FileLockHandle.ReleaseAsync(token);
//            await s3FileLockHandle.ReleaseAsync(token); //Checking that the lock is only released once

//            // Assert
//            this.mockRepository.VerifyAll();
//        }

//        [Fact]
//        public async Task DisposeAsync_Should_ReleaseLock()
//        {
//            // Act
//            await s3FileLockHandle.DisposeAsync();
//            await s3FileLockHandle.DisposeAsync(); //Checking that the lock is only released once

//            // Assert
//            this.mockRepository.VerifyAll();
//        }
//    }
//}
