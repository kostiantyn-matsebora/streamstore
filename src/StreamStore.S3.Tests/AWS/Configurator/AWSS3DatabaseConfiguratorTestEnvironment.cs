﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.S3.AWS;
using StreamStore.S3.Client;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.AWS.Configurator
{
    public class AWSS3DatabaseConfiguratorTestEnvironment : TestEnvironmentBase
    {
        public MockRepository MockRepository { get; }

        public Mock<IServiceCollection> ServiceCollection { get; }

        public AWSS3DatabaseConfigurator CreateAWSDatabaseConfigurator()
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
                 d.ServiceType == typeof(IStreamDatabase) &&
                 d.ImplementationType == typeof(S3StreamDatabase) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );
            ServiceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IAmazonS3ClientFactory) &&
                 d.ImplementationType == typeof(AmazonS3ClientFactory) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );

            return new AWSS3DatabaseConfigurator(ServiceCollection.Object);
        }

        public AWSS3DatabaseConfiguratorTestEnvironment()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            ServiceCollection = MockRepository.Create<IServiceCollection>();
        }
    }
}
