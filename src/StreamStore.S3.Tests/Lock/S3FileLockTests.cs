using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.S3.Storage;
using StreamStore.Serialization;
using StreamStore.Testing;


namespace StreamStore.S3.Tests.Lock
{
    public class S3FileLockTests
    {
        readonly MockRepository mockRepository;
        readonly S3LockObject lockObject;
        readonly Mock<IS3ClientFactory> factory;
        readonly Id transactionId;
        readonly S3ContainerPath path;

        public S3FileLockTests()
        {
            var fixture = new Fixture();
            path = fixture.Create<S3ContainerPath>();
            mockRepository = new MockRepository(MockBehavior.Strict);
            transactionId = Generated.Id;
            factory = this.mockRepository.Create<IS3ClientFactory>();
            lockObject = new S3LockObject(path, factory.Object);
        }

        S3FileLock CreateS3FileLock()
        {
            return new S3FileLock(lockObject, transactionId);
        }

        [Fact]
        public async Task AcquireAsync_Should_AcquireLock()
        {
            // Arrange
            var s3FileLock = this.CreateS3FileLock();
            CancellationToken token = default;
            var lockId = new LockId(transactionId);
            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();
            factory.Setup(x => x.CreateClient()).Returns(client.Object);


            var uploadResponse = new UploadObjectResponse
            {
                Name = path
            };

            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(lockId),
                Name = path,
            };

            client.
                SetupSequence(x => x.FindObjectAsync(path, token))
                .ReturnsAsync((FindObjectResponse?)null)
                .ReturnsAsync(response);

            client
                .Setup(x =>
                    x.UploadObjectAsync(
                        It.Is<UploadObjectRequest>(r => r.Key == path),
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

            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();

            factory.Setup(x => x.CreateClient()).Returns(client.Object);
           


            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new LockId(Generated.String)),
                Name = path,
            };

            client.
                Setup(x => x.FindObjectAsync(path, token))
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

            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();

            factory.Setup(x => x.CreateClient()).Returns(client.Object);

            client.
                 Setup(x => x.FindObjectAsync(path, token))
                .ReturnsAsync((FindObjectResponse?)null);

            client
                 .Setup(x =>
                     x.UploadObjectAsync(
                         It.Is<UploadObjectRequest>(r => r.Key == path),
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

            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();

            factory.Setup(x => x.CreateClient()).Returns(client.Object);

            var uploadResponse = new UploadObjectResponse
            {
                Name = path,
            };

            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new LockId(Generated.String)),
                Name = path,
            };

            client.
                 SetupSequence(x => x.FindObjectAsync(path, token))
                .ReturnsAsync((FindObjectResponse?)null)
                .ReturnsAsync(response);

            client
                 .Setup(x =>
                     x.UploadObjectAsync(
                         It.Is<UploadObjectRequest>(r => r.Key ==path),
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
