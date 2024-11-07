using System.IO;
using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.Storage
{
    public class S3StorageTestSuite : TestSuiteBase
    {

        public MockRepository MockRepository { get; }
        public Mock<IS3Client> MockS3Client { get; }
        public Mock<IS3ClientFactory> MockS3ClientFactory { get; }
        internal S3ContainerPath Path { get; }


        internal S3EventStorage CreateS3EventStorage()
        {
            return new S3EventStorage(Path, MockS3ClientFactory.Object);
        }

        internal S3ObjectContainer CreateS3ObjectContainer()
        {
            return new S3ObjectContainer(Path, MockS3ClientFactory.Object);
        }

        internal S3BinaryObject CreateS3Object()
        {
            return new S3BinaryObject(Path, MockS3ClientFactory.Object);
        }

        public S3StorageTestSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
            MockS3Client = new Mock<IS3Client>(MockBehavior.Strict);
            MockS3ClientFactory = MockRepository.Create<IS3ClientFactory>();
            Path = new S3ContainerPath(Generated.String);

            MockS3ClientFactory
                .Setup(x => x.CreateClient())
                .Returns(MockS3Client.Object);
        }
    }
}
