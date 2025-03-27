using Amazon.S3;
using FluentAssertions;
using Moq;
using StreamStore.S3.AWS;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS.Factory
{
    public class Creating_client: Scenario<AWSS3FactoryTestEnvironment>
    {

        [Fact]
        public void When_settings_is_not_set()
        {
            // Act && Assert
            Action act = () => new AWSS3Factory(null!, null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_creating_client()
        {
            // Arrange
            var amazonFactory = new Mock<IAmazonS3ClientFactory>();
            amazonFactory.Setup(x => x.CreateClient()).Returns(new Mock<IAmazonS3>().Object);
            var factory = Environment.CreateFactory(amazonFactory.Object);

            // Act
            factory.CreateClient().Should().NotBeNull();
        }
    }
}
