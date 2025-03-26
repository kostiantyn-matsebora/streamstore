using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.Concurrency.StreamContext
{
    public class S3StreamContextSuite: TestSuiteBase
    {
        public MockRepository MockRepository { get; }
        internal Mock<IS3TransactionalStorage> MockTransactionalStorage { get; }
        internal S3StreamContainer Transient { get;  }
        internal S3StreamContainer Persistent { get; }

        internal  Mock<IS3ClientFactory> MockClientFactory { get; }

        internal Mock<IS3Client> MockClient { get; }

        public S3StreamContextSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
            MockTransactionalStorage = new Mock<IS3TransactionalStorage>(MockBehavior.Strict);
            MockClientFactory = new Mock<IS3ClientFactory>(MockBehavior.Strict);
            MockClient = new Mock<IS3Client>(MockBehavior.Strict);
            MockClientFactory.Setup(x => x.CreateClient()).Returns(MockClient.Object);
            Transient = new S3StreamContainer(Generated.Primitives.String, MockClientFactory.Object);
            Persistent = new S3StreamContainer(Generated.Primitives.String, MockClientFactory.Object);
            MockTransactionalStorage.Setup(x => x.GetTransientContainer(It.IsAny<string>())).Returns(Transient);
            MockTransactionalStorage.Setup(x => x.GetPersistentContainer(It.IsAny<string>())).Returns(Persistent);
        }

        internal S3StreamContext CreateStreamContext(Id streamId, Revision expectedRevision)
        {
            return new S3StreamContext(streamId, expectedRevision, MockTransactionalStorage.Object);
        }
    }
}
