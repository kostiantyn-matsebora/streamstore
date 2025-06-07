//using FluentAssertions;

//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.S3.AWS;
//using StreamStore.Testing;

//namespace StreamStore.S3.Tests.AWS.Configurator
//{
//    public class Configuring_storage : Scenario<AWSS3StorageConfiguratorTestEnvironment>
//    {
//        [Fact]
//        public void When_begining_configuration()
//        {
//            // Arrange
//            var collection = Environment.MockRepository.Create<IServiceCollection>();

//            // Act
//            var result = Environment.CreateAWSStorageConfigurator();

//            // Assert
//            result.Should().NotBeNull();
//            result.Should().BeOfType<AWSS3StorageConfigurator>();
//            Environment.MockRepository.VerifyAll();
//        }

//        [Fact]
//        public void When_using_store_configurator_for_reading_from_app_config()
//        {
//            // Arrange
//            var collection = new ServiceCollection();

//            // Act
//            collection.ConfigureStreamStore(x => x.WithSingleStorage(x => x.UseAWSStorage()));

//            // Assert
//            Environment.MockRepository.VerifyAll();
//        }
//    }
//}
