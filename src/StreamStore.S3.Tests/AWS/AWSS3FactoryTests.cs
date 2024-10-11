using Amazon.S3;
using FluentAssertions;
using Moq;
using StreamStore.S3.AWS;


namespace StreamStore.S3.Tests.AWS
{
    public class AWSS3FactoryTests
    {
        readonly AWSS3DatabaseSettings databaseSettings;

        public AWSS3FactoryTests()
        {
            databaseSettings = new AWSS3DatabaseSettingsBuilder().Build();
        }

        AWSS3Factory CreateFactory(IAmazonS3ClientFactory clientFactory)
        {
            return new AWSS3Factory(databaseSettings, clientFactory);
        }

        [Fact]
        public void CreateClient_Should_ThrowArgumentNullIfSettingsNotSet()
        {
            // Act && Assert
            Action act = () => new AWSS3Factory(null!, null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateClient_Should_CreateClient()
        {
            // Arrange
            var amazonFactory = new Mock<IAmazonS3ClientFactory>();
            amazonFactory.Setup(x => x.CreateClient()).Returns(new Mock<IAmazonS3>().Object);
            var factory = CreateFactory(amazonFactory.Object);

            // Act
            factory.CreateClient().Should().NotBeNull();
        }
    }
}
