using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.B2;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Configurator
{
    public class Configuring_database: Scenario<B2S3DatabaseConfiguratorTestEnvironment>
    {

        [Fact]
        public void When_credentials_not_set()
        {
            // Arrange
            var b2DatabaseConfigurator = Environment.CreateB2DatabaseConfigurator();

            // Act
            Action act = () => b2DatabaseConfigurator.Configure();

            // Assert
            Environment.MockRepository.VerifyAll();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_reading_from_app_config()
        {
            // Arrange
            var section = Environment.SetupConfiguration();
            var b2DatabaseConfigurator = Environment.CreateB2DatabaseConfigurator();

            // Act
            b2DatabaseConfigurator.ReadFromConfig(section.Object);

            // Assert
            Environment.MockRepository.VerifyAll();
            section.VerifyAll();
        }

        [Fact]
        public void When_using_parametereless_configurator()
        {
            // Arrange
            var collection = Environment.MockRepository.Create<IServiceCollection>();
            var configuration = Environment.SetupConfiguration();
            var b2DatabaseConfigurator = Environment.CreateB2DatabaseConfigurator();

            // Act
            b2DatabaseConfigurator.ReadFromConfig(configuration.Object);

            // Assert
            Environment.MockRepository.VerifyAll();
            configuration.VerifyAll();
        }

        [Fact]
        public void When_begining_configuration()
        {
            // Arrange
            var collection = Environment.MockRepository.Create<IServiceCollection>();

            // Act
            var result = Environment.CreateB2DatabaseConfigurator();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<B2DatabaseConfigurator>();
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public void When_using_store_configurator_for_reading_from_app_config()
        {
            // Arrange
            var section = Environment.SetupConfiguration();
            var collection = new ServiceCollection();

            // Act
            collection.ConfigureStreamStore(x => x.WithSingleDatabase(x => x.UseB2Database(section.Object)));

            // Assert
            Environment.MockRepository.VerifyAll();
            section.VerifyAll();
        }

        [Fact]
        public void When_using_store_configurator_for_configuring_manually()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            collection.ConfigureStreamStore(x => x.WithSingleDatabase(x => x.UseB2Database(c => c.WithCredential(Generated.Primitives.String, Generated.Primitives.String).WithBucketId(Generated.Primitives.String))));

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
