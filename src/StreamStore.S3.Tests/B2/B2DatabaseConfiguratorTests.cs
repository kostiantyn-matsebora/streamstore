﻿using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.Testing;


namespace StreamStore.S3.Tests.B2
{
    public class B2DatabaseConfiguratorTests
    {
        readonly MockRepository mockRepository;

        readonly Mock<IServiceCollection> serviceCollection;

        public B2DatabaseConfiguratorTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);

            serviceCollection = mockRepository.Create<IServiceCollection>();
        }

        B2DatabaseConfigurator CreateB2DatabaseConfigurator()
        {
            serviceCollection.Setup(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.ServiceType == typeof(IS3Factory) &&
                    d.ImplementationType == typeof(B2S3Factory) &&
                    d.Lifetime == ServiceLifetime.Singleton))
                );
            serviceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IStreamDatabase) &&
                 d.ImplementationType == typeof(S3StreamDatabase) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );
            serviceCollection.Setup(
             x => x.Add(It.Is<ServiceDescriptor>(d =>
                 d.ServiceType == typeof(IStorageClientFactory) &&
                 d.ImplementationType == typeof(BackblazeClientFactory) &&
                 d.Lifetime == ServiceLifetime.Singleton))
             );

            return new B2DatabaseConfigurator(serviceCollection.Object);
           
        }

        [Fact]
        public void Configure_Should_Throw_ExceptionIfCredentialsAreNotSet()
        {
            // Arrange
            var b2DatabaseConfigurator = CreateB2DatabaseConfigurator();
          
            // Act
            Action act = () => b2DatabaseConfigurator.Configure();

            // Assert
            mockRepository.VerifyAll();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ReadFromConfig_Should_ReadFromConfiguration()
        {

            var applicationKeyId = GeneratedValues.String;
            var applicationKey = GeneratedValues.String;
            var bucketId = GeneratedValues.String;
            var bucketName = GeneratedValues.String;

            // Arrange
            var b2DatabaseConfigurator = CreateB2DatabaseConfigurator();
            var configuration = mockRepository.Create<IConfiguration>();
            var section = new Mock<IConfigurationSection>(MockBehavior.Loose);
            configuration.Setup(c => c.GetSection("streamStore:b2")).Returns(section.Object).Verifiable();

            var applicationKeyIdSection = mockRepository.Create<IConfigurationSection>();
            applicationKeyIdSection.SetupGet(s => s.Value).Returns(applicationKeyId).Verifiable();
            var applicationKeySection = mockRepository.Create<IConfigurationSection>();
            applicationKeySection.SetupGet(s => s.Value).Returns(applicationKey);
            var bucketIdSection = mockRepository.Create<IConfigurationSection>();
            bucketIdSection.SetupGet(s => s.Value).Returns(bucketId).Verifiable();
            var bucketNameSection = mockRepository.Create<IConfigurationSection>();
            bucketNameSection.SetupGet(s => s.Value).Returns(bucketName).Verifiable();

            section.Setup(s => s.GetSection("applicationKeyId")).Returns(applicationKeyIdSection.Object).Verifiable();
            section.Setup(s => s.GetSection("applicationKey")).Returns(applicationKeySection.Object).Verifiable();
            section.Setup(s => s.GetSection("bucketId")).Returns(bucketIdSection.Object).Verifiable();
            section.Setup(s => s.GetSection("bucketName")).Returns(bucketNameSection.Object).Verifiable();


            // Act
            var result = b2DatabaseConfigurator.ReadFromConfig(configuration.Object);

            // Assert
            mockRepository.VerifyAll();
            section.VerifyAll();
        }
    }
}