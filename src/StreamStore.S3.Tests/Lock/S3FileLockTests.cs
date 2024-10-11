using System.Transactions;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Lock;
using StreamStore.Testing;


namespace StreamStore.S3.Tests.Lock
{
    public class S3FileLockTests
    {
        readonly MockRepository mockRepository;
        readonly Mock<IS3TransactionContext> ctx;
        readonly Mock<IS3Factory> factory;

        public S3FileLockTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.ctx = this.mockRepository.Create<IS3TransactionContext>();
            this.factory = this.mockRepository.Create<IS3Factory>();
        }

        S3FileLock CreateS3FileLock()
        {
            return new S3FileLock(
                this.ctx.Object,
                this.factory.Object);
        }

        [Fact]
        public async Task AcquireAsync_Should_AcquireLock()
        {
            // Arrange
            var s3FileLock = this.CreateS3FileLock();
            CancellationToken token = default;
            var transactionId = new Id(GeneratedValues.String);
            var lockId = new LockId(transactionId);
            var lockKey = GeneratedValues.String;
            var client = new Mock<IS3Client>();

            factory.Setup(x => x.CreateClient()).Returns(client.Object);
            ctx.Setup(x => x.LockKey).Returns(lockKey);
            ctx.Setup(x => x.TransactionId).Returns(transactionId);

            var uploadResponse = new UploadObjectResponse
            {
                Name = ctx.Object.LockKey,
            };

            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(lockId),
                Name = ctx.Object.LockKey,
            };

            client.
                SetupSequence(x => x.FindObjectAsync(ctx.Object.LockKey, token))
                .ReturnsAsync((FindObjectResponse?)null)
                .ReturnsAsync(response);

            client
                .Setup(x =>
                    x.UploadObjectAsync(
                        It.Is<UploadObjectRequest>(r => r.Key == ctx.Object.LockKey),
                        token)
                ).ReturnsAsync(uploadResponse);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().NotBeNull();
            result!.Should().BeOfType<S3FileLockHandle>();
            client.VerifyAll();
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AcquireAsync_Should_NotAcquireLockIfExists()
        {
            // Arrange
            var s3FileLock = this.CreateS3FileLock();
            CancellationToken token = default;

            var lockKey = GeneratedValues.String;
            var client = new Mock<IS3Client>();

            factory.Setup(x => x.CreateClient()).Returns(client.Object);
            ctx.Setup(x => x.LockKey).Returns(lockKey);


            var response = new FindObjectResponse
            {
                Data = GeneratedValues.ByteArray,
                Name = ctx.Object.LockKey,
            };

            client.
                Setup(x => x.FindObjectAsync(ctx.Object.LockKey, token))
                .ReturnsAsync(response);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().BeNull();
            client.VerifyAll();
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AcquireAsync_Should_NotAcquireLockIfUploadFailed()
        {
            // Arrange
            var s3FileLock = this.CreateS3FileLock();
            CancellationToken token = default;

            var lockKey = GeneratedValues.String;
            var client = new Mock<IS3Client>();

            factory.Setup(x => x.CreateClient()).Returns(client.Object);
            ctx.Setup(x => x.LockKey).Returns(lockKey);
            ctx.Setup(x => x.TransactionId).Returns(GeneratedValues.String);


            client.
                 Setup(x => x.FindObjectAsync(ctx.Object.LockKey, token))
                .ReturnsAsync((FindObjectResponse?)null);

            client
                 .Setup(x =>
                     x.UploadObjectAsync(
                         It.Is<UploadObjectRequest>(r => r.Key == ctx.Object.LockKey),
                         token)
                 ).ReturnsAsync((UploadObjectResponse?)null);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().BeNull();
            client.VerifyAll();
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AcquireAsync_Should_NotAcquireLockIfLockedByAnotherTransaction()
        {
            // Arrange
            var s3FileLock = this.CreateS3FileLock();
            CancellationToken token = default;

            var lockKey = GeneratedValues.String;
            var client = new Mock<IS3Client>();
            var transactionId = new Id(GeneratedValues.String);

            factory.Setup(x => x.CreateClient()).Returns(client.Object);
            ctx.Setup(x => x.LockKey).Returns(lockKey);
            ctx.Setup(x => x.TransactionId).Returns(transactionId);

            var uploadResponse = new UploadObjectResponse
            {
                Name = ctx.Object.LockKey,
            };

            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new LockId(GeneratedValues.String)),
                Name = ctx.Object.LockKey,
            };

            client.
                 SetupSequence(x => x.FindObjectAsync(ctx.Object.LockKey, token))
                .ReturnsAsync((FindObjectResponse?)null)
                .ReturnsAsync(response);

            client
                 .Setup(x =>
                     x.UploadObjectAsync(
                         It.Is<UploadObjectRequest>(r => r.Key == ctx.Object.LockKey),
                         token)
                 ).ReturnsAsync((UploadObjectResponse?)uploadResponse);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().BeNull();
            client.VerifyAll();
            mockRepository.VerifyAll();
        }
    }
}
