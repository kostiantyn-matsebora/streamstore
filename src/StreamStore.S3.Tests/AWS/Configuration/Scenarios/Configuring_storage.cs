using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;
using StreamStore.S3.AWS;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace StreamStore.S3.Tests.AWS.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            return new StorageConfigurator(new AWSS3StorageSettings());
        }


        [Fact]
        public void Configuring_by_extension()
        {
            // Arrange
            var services = new ServiceCollection();


            // Act
            services.AddAmazonS3(builder => builder.WithBucketName("test-bucket"));

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetService<IStreamStorage>()
                    .Should().NotBeNull();

            var settings = provider.GetService<AWSS3StorageSettings>();
            settings.Should().NotBeNull();
            settings.BucketName.Should().Be("test-bucket");
        }
    }
}
