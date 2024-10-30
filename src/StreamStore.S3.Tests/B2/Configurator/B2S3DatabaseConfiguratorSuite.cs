
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.B2.Configurator
{
    public class B2S3DatabaseConfiguratorSuite: TestSuiteBase
    {
        public MockRepository MockRepository { get; }

        public Mock<IServiceCollection> ServiceCollection { get;  }

        public B2DatabaseConfigurator CreateB2DatabaseConfigurator()
        {
           ServiceCollection.Setup(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.ServiceType == typeof(IS3ClientFactory) &&
                    d.ImplementationType == typeof(B2S3Factory) &&
                    d.Lifetime == ServiceLifetime.Singleton))
                );
            ServiceCollection.Setup(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.ServiceType == typeof(IS3LockFactory) &&
                    d.ImplementationType == typeof(B2S3Factory) &&
                    d.Lifetime == ServiceLifetime.Singleton))
                );
            ServiceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IStreamDatabase) &&
                 d.ImplementationType == typeof(S3StreamDatabase) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );
            ServiceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IStorageClientFactory) &&
                 d.ImplementationType == typeof(BackblazeClientFactory) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );

            return new B2DatabaseConfigurator(ServiceCollection.Object);
        }

        public Mock<IConfiguration> SetupConfiguration()
        {
            var applicationKeyId = Generated.String;
            var applicationKey = Generated.String;
            var bucketId = Generated.String;
            var bucketName = Generated.String;

            // Arrange

            var configuration = MockRepository.Create<IConfiguration>();
            var section = new Mock<IConfigurationSection>(MockBehavior.Loose);
            configuration.Setup(c => c.GetSection("streamStore:b2")).Returns(section.Object).Verifiable();

            var applicationKeyIdSection = MockRepository.Create<IConfigurationSection>();
            applicationKeyIdSection.SetupGet(s => s.Value).Returns(applicationKeyId).Verifiable();
            var applicationKeySection = MockRepository.Create<IConfigurationSection>();
            applicationKeySection.SetupGet(s => s.Value).Returns(applicationKey);
            var bucketIdSection = MockRepository.Create<IConfigurationSection>();
            bucketIdSection.SetupGet(s => s.Value).Returns(bucketId).Verifiable();
            var bucketNameSection = MockRepository.Create<IConfigurationSection>();
            bucketNameSection.SetupGet(s => s.Value).Returns(bucketName).Verifiable();

            section.Setup(s => s.GetSection("applicationKeyId")).Returns(applicationKeyIdSection.Object).Verifiable();
            section.Setup(s => s.GetSection("applicationKey")).Returns(applicationKeySection.Object).Verifiable();
            section.Setup(s => s.GetSection("bucketId")).Returns(bucketIdSection.Object).Verifiable();
            section.Setup(s => s.GetSection("bucketName")).Returns(bucketNameSection.Object).Verifiable();
            return configuration;
        }

        public B2S3DatabaseConfiguratorSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            ServiceCollection = MockRepository.Create<IServiceCollection>();
        }
    }
}
