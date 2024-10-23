using AutoFixture;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.S3.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.Lock.File
{
    public class S3FileLockSuite : TestSuite
    {
        internal S3LockObject LockObject { get; }

        public Mock<IS3ClientFactory> Factory { get; }

        public Id TransactionId { get; }
        internal S3ContainerPath Path { get; }

        internal S3FileLock CreateS3FileLock()
        {
            return new S3FileLock(LockObject, TransactionId);
        }

        internal S3FileLockHandle CreateS3FileLockHandle()
        {
            return new S3FileLockHandle(LockObject);
        }

        public S3FileLockSuite()
        {
            var fixture = new Fixture();
            Path = fixture.Create<S3ContainerPath>();

            TransactionId = Generated.Id;
            Factory = MockRepository.Create<IS3ClientFactory>();
            LockObject = new S3LockObject(Path, Factory.Object);

            var client = new Mock<IS3Client>();
            Factory.Setup(x => x.CreateClient()).Returns(client.Object);
        }
    }
}
