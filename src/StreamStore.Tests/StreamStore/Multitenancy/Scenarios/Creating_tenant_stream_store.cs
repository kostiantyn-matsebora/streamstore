using FluentAssertions;
using Moq;
using StreamStore.Multitenancy;
using StreamStore.Testing;
using Environment = StreamStore.Tests.StreamStore.Multitenancy.TenantStreamStoreFactoryTestEnvironment;
namespace StreamStore.Tests.StreamStore.Multitenancy
{
    public class Creating_tenant_stream_store : Scenario<TenantStreamStoreFactoryTestEnvironment>
    {

        [Fact]
        public void When_one_of_the_parameters_is_null()
        {

            // Act
            var act = () => new TenantStreamStoreFactory(null!, Environment.MockTenantStreamDatabaseProvider.Object, Environment.MockEventSerializer.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("configuration");

            // Act
            act = () => new TenantStreamStoreFactory(Environment.Configuration, null!, Environment.MockEventSerializer.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("databaseProvider");

            // Act
            act = () => new TenantStreamStoreFactory(Environment.Configuration, Environment.MockTenantStreamDatabaseProvider.Object, null!);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("serializer");

        }

        [Fact]
        public void When_streamstore_is_created()
        {
            // Arrange
            var databaseProvider = Environment.MockTenantStreamDatabaseProvider;
            databaseProvider.Setup(x => x.GetDatabase(It.IsAny<Id>())).Returns(Generated.Mocks.Single<IStreamDatabase>().Object);
            var factory = new TenantStreamStoreFactory(Environment.Configuration, databaseProvider.Object, Environment.MockEventSerializer.Object);

            // Act
            var store = factory.Create(Generated.Primitives.Id);

            //Assert
            store.Should().NotBeNull();
        }
    }
}
