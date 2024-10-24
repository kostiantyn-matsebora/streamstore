using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore;
using StreamStore.InMemory;
using StreamStore.Serialization;
using StreamStore.Testing;


namespace StreamStore.Tests.ServiceCollection
{
    public class Register_components : Scenario<ServiceCollectionSuite>
    {
        [Fact]
        public void When_register_in_memory_database_to_container()
        {
            // Arrange
            Suite.MockServices.Setup(m => m.Add(It.Is<ServiceDescriptor>(
                s => s.ServiceType == typeof(IStreamDatabase) &&
                     s.ImplementationType == typeof(InMemoryStreamDatabase) &&
                     s.Lifetime == ServiceLifetime.Singleton)
            ));

            // Act
            Suite.MockServices.Object.AddInMemoryStreamDatabase();
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public void When_registering_in_memory_database()
        {
            // Arrange
            Suite.MockServices.Setup(m => m.Add(It.Is<ServiceDescriptor>(
                     s => s.ServiceType == typeof(IStreamStore) &&
                     s.Lifetime == ServiceLifetime.Singleton)
            ));

            Suite.MockServices.Setup(m => m.Add(It.Is<ServiceDescriptor>(
                    s => s.ServiceType == typeof(IEventSerializer) &&
                    s.Lifetime == ServiceLifetime.Singleton)
            ));

            Suite.MockServices.Setup(m => m.Add(It.Is<ServiceDescriptor>(
                   s => s.ServiceType == typeof(ITypeRegistry) &&
                   s.Lifetime == ServiceLifetime.Singleton)
           ));


            // Act
            ServiceCollectionExtension.ConfigureStreamStore(Suite.MockServices.Object);
            Suite.MockRepository.VerifyAll();
        }
    }
}
