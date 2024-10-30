using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.B2;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Configurator
{
    public class Configuring_database: Scenario<B2S3DatabaseConfiguratorSuite>
    {

        [Fact]
        public void When_credentials_not_set()
        {
            // Arrange
            var b2DatabaseConfigurator = Suite.CreateB2DatabaseConfigurator();

            // Act
            Action act = () => b2DatabaseConfigurator.Configure();

            // Assert
            Suite.MockRepository.VerifyAll();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_reading_from_app_config()
        {
            // Arrange
            var section = Suite.SetupConfiguration();
            var b2DatabaseConfigurator = Suite.CreateB2DatabaseConfigurator();

            // Act
            b2DatabaseConfigurator.ReadFromConfig(section.Object);

            // Assert
            Suite.MockRepository.VerifyAll();
            section.VerifyAll();
        }

        [Fact]
        public void When_using_parametereless_configurator()
        {
            // Arrange
            var collection = Suite.MockRepository.Create<IServiceCollection>();
            var configuration = Suite.SetupConfiguration();

            // Act
            collection.Object.UseB2StreamStoreDatabase(configuration.Object);

            // Assert
            Suite.MockRepository.VerifyAll();
            configuration.VerifyAll();
        }

        [Fact]
        public void When_begining_configuration()
        {
            // Arrange
            var collection = Suite.MockRepository.Create<IServiceCollection>();

            // Act
            var result = collection.Object.ConfigureB2StreamStoreDatabase();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<B2DatabaseConfigurator>();
            Suite.MockRepository.VerifyAll();
        }
    }
}
