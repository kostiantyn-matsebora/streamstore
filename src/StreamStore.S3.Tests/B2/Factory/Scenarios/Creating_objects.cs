using Bytewizer.Backblaze.Client;
using Moq;
using StreamStore.S3.B2;
using StreamStore.Testing;
using FluentAssertions;

namespace StreamStore.S3.Tests.B2.Factory { 
    public class Creating_objects: Scenario<B2S3FactorySuite>
    {

        [Fact]
        public void When_creating_client()
        {
            // Arrange

            var client = new Mock<IStorageClient>();
            Suite.FactoryMock.Setup(m => m.Create()).Returns(client.Object);
            client.Setup(client => client.Connect(Suite.Settings.Credentials!.AccessKeyId, Suite.Settings.Credentials!.AccessKey));
            // Act
            var factory = new B2S3Factory(Suite.Settings, Suite.FactoryMock.Object);
            var result = factory.CreateClient();

            // Assert
            Suite.MockRepository.VerifyAll();
            Suite.FactoryMock.VerifyAll();
        }

        [Fact]
        public void When_creating_lock()
        {
            // Arrange
            var client = new Mock<IStorageClient>();
            Suite.FactoryMock.Setup(m => m.Create()).Returns(client.Object);
            client.Setup(client => client.Connect(Suite.Settings.Credentials!.AccessKeyId, Suite.Settings.Credentials!.AccessKey));

            // Act
            var factory = new B2S3Factory(Suite.Settings, Suite.FactoryMock.Object);
            var result = factory.CreateLock(Suite.StreamId, Suite.TransactionId);

            // Assert
            result.Should().NotBeNull();
            Suite.FactoryMock.VerifyAll();
        }
    }
}
