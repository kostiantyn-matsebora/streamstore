using FluentAssertions;
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

        AWSS3Factory CreateFactory()
        {
            return new AWSS3Factory(databaseSettings);
        }

        [Fact]
        public void CreateClient_Should_ThrowArgumentNullIfSettingsNotSet()
        {
            // Act && Assert
            Action act = () => new AWSS3Factory(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateClient_Should_CreateClient()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            factory.CreateClient().Should().NotBeNull();
        }
    }
}
