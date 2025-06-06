using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.S3.AWS;
using StreamStore.S3.Client;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.AWS.Configurator
{
    public class AWSS3StorageConfiguratorTestEnvironment : TestEnvironmentBase
    {
        public MockRepository MockRepository { get; }

        public Mock<IServiceCollection> ServiceCollection { get; }

        public AWSS3StorageConfigurator CreateAWSStorageConfigurator()
        {
            ServiceCollection.Setup(
                 x => x.Add(It.Is<ServiceDescriptor>(d =>
                     d.ServiceType == typeof(IS3ClientFactory) &&
                     d.ImplementationType == typeof(AWSS3Factory) &&
                     d.Lifetime == ServiceLifetime.Singleton))
                 );
            ServiceCollection.Setup(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.ServiceType == typeof(IS3LockFactory) &&
                    d.ImplementationType == typeof(AWSS3Factory) &&
                    d.Lifetime == ServiceLifetime.Singleton))
                );
            ServiceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IStreamStorage) &&
                 d.ImplementationType == typeof(S3StreamStorage) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );
            ServiceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IAmazonS3ClientFactory) &&
                 d.ImplementationType == typeof(AmazonS3ClientFactory) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );

            return new AWSS3StorageConfigurator(ServiceCollection.Object, new AWSS3StorageConfigurationBuilder().Build());
        }

        public AWSS3StorageConfiguratorTestEnvironment()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            ServiceCollection = MockRepository.Create<IServiceCollection>();
        }
    }
}
