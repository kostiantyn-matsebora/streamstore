using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.Testing;
using StreamStore.Serialization;
using FluentAssertions;


namespace StreamStore.S3.Tests.Lock.File.Scenarios
{
    public class Acquiring_lock : Scenario<S3FileLockSuite>
    {

        public Acquiring_lock() : base(new S3FileLockSuite())
        {
        }

        [Fact]
        public async Task When_lock_is_acquired()
        {
            // Arrange
            var s3FileLock = Suite.CreateS3FileLock();
            CancellationToken token = default;
            var lockId = new LockId(Suite.TransactionId);
            var lockKey = Generated.String;

            var client = new Mock<IS3Client>();

            Suite.Factory.Setup(x => x.CreateClient()).Returns(client.Object);


            var uploadResponse = new UploadObjectResponse
            {
                Name = Suite.Path
            };

            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(lockId),
                Name = Suite.Path,
            };

            client.
                SetupSequence(x => x.FindObjectAsync(Suite.Path, token))
                .ReturnsAsync((FindObjectResponse?)null)
                .ReturnsAsync(response);

            client
                .Setup(x =>
                    x.UploadObjectAsync(
                        It.Is<UploadObjectRequest>(r => r.Key == Suite.Path),
                        token)
                ).ReturnsAsync(uploadResponse);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().NotBeNull();
            result!.Should().BeOfType<S3FileLockHandle>();
            client.VerifyAll();
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_lock_already_exists()
        {
            // Arrange
            var s3FileLock = Suite.CreateS3FileLock();
            CancellationToken token = default;

            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();

            Suite.Factory.Setup(x => x.CreateClient()).Returns(client.Object);



            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new LockId(Generated.String)),
                Name = Suite.Path,
            };

            client.
                Setup(x => x.FindObjectAsync(Suite.Path, token))
                .ReturnsAsync(response);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().BeNull();
            client.VerifyAll();
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_lock_file_upload_failed()
        {
            // Arrange
            var s3FileLock = Suite.CreateS3FileLock();
            CancellationToken token = default;

            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();

            Suite.Factory.Setup(x => x.CreateClient()).Returns(client.Object);

            client.
                 Setup(x => x.FindObjectAsync(Suite.Path, token))
                .ReturnsAsync((FindObjectResponse?)null);

            client
                 .Setup(x =>
                     x.UploadObjectAsync(
                         It.Is<UploadObjectRequest>(r => r.Key == Suite.Path),
                         token)
                 ).ReturnsAsync((UploadObjectResponse?)null);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().BeNull();
            client.VerifyAll();
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_stream_locked_by_another_transaction()
        {
            // Arrange
            var s3FileLock = Suite.CreateS3FileLock();
            CancellationToken token = default;

            var lockKey = Generated.String;
            var client = new Mock<IS3Client>();

            Suite.Factory.Setup(x => x.CreateClient()).Returns(client.Object);

            var uploadResponse = new UploadObjectResponse
            {
                Name = Suite.Path,
            };

            var response = new FindObjectResponse
            {
                Data = Converter.ToByteArray(new LockId(Generated.String)),
                Name = Suite.Path,
            };
            client.
                 SetupSequence(x => x.FindObjectAsync(Suite.Path, token))
                .ReturnsAsync((FindObjectResponse?)null)
                .ReturnsAsync(response);

            client
                 .Setup(x =>
                     x.UploadObjectAsync(
                         It.Is<UploadObjectRequest>(r => r.Key == Suite.Path),
                         token)
                 ).ReturnsAsync((UploadObjectResponse?)uploadResponse);

            // Act
            var result = await s3FileLock.AcquireAsync(token);

            // Assert
            result.Should().BeNull();
            client.VerifyAll();
            Suite.MockRepository.VerifyAll();
        }
    }
}
