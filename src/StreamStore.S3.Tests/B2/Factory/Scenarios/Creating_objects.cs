using Bytewizer.Backblaze.Client;
using Moq;
using StreamStore.S3.B2;
using StreamStore.Testing;
using FluentAssertions;

namespace StreamStore.S3.Tests.B2.Factory { 
    public class Creating_objects: Scenario<B2S3FactoryTestEnvironment>
    {

        [Fact]
        public void When_creating_client()
        {
            // Arrange

            var client = new Mock<IStorageClient>();
            Environment.FactoryMock.Setup(m => m.Create()).Returns(client.Object);
            client.Setup(client => client.Connect(Environment.Settings.Credential!.UserName, Environment.Settings.Credential!.Password));

            // Act
            var factory = new B2S3Factory(Environment.Settings, Environment.FactoryMock.Object);
            var result = factory.CreateClient();

            // Assert
            Environment.MockRepository.VerifyAll();
            Environment.FactoryMock.VerifyAll();
        }

        [Fact]
        public void When_creating_lock()
        {
            // Arrange
            var client = new Mock<IStorageClient>();

            // Act
            var factory = new B2S3Factory(Environment.Settings, Environment.FactoryMock.Object);
            var result = factory.CreateLock(Environment.StreamId, Environment.TransactionId);

            // Assert
            result.Should().NotBeNull();
            Environment.FactoryMock.VerifyAll();
        }
    }
}
