using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.InMemory;
using StreamStore.Serialization;


namespace StreamStore.Tests
{
    public class ServiceCollectionExtensionTests
    {
        MockRepository mockRepository;
        Mock<IServiceCollection> services;
        public ServiceCollectionExtensionTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            services = mockRepository.Create<IServiceCollection>();
        }

        [Fact]
        public void AddInMemoryStreamDatabase_Should_RegisterDatabase()
        {
            // Arrange
            services.Setup(m => m.Add(It.Is<ServiceDescriptor>(
                s => s.ServiceType == typeof(IStreamDatabase) &&
                     s.ImplementationType == typeof(InMemoryStreamDatabase) &&
                     s.Lifetime == ServiceLifetime.Singleton)
                ));

            // Act
            InMemory.ServiceCollectionExtension.AddInMemoryStreamDatabase(services.Object);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void ConfigureStreamStore_Should_RegisterStore()
        {
            // Arrange
            services.Setup(m => m.Add(It.Is<ServiceDescriptor>(
                s => s.ServiceType == typeof(IStreamStore) &&
                     s.ImplementationType == typeof(StreamStore) &&
                     s.Lifetime == ServiceLifetime.Singleton)
                ));

            services.Setup(m => m.Add(It.Is<ServiceDescriptor>(
            s => s.ServiceType == typeof(IEventSerializer) &&
                    s.ImplementationType == typeof(NewtonsoftEventSerializer) &&
                    s.Lifetime == ServiceLifetime.Singleton)
            ));

            // Act
            ServiceCollectionExtension.ConfigureStreamStore(services.Object);
            mockRepository.VerifyAll();
        }
    }
}
