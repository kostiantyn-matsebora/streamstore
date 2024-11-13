using FluentAssertions;
using Moq;
using StreamStore.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.Tests.StreamStore.Multitenancy
{
    public class Creating_tenant_stream_store : Scenario<TenantStreamStoreFactorySuite>
    {

        [Fact]
        public void When_one_of_the_parameters_is_null()
        {

            // Act
            var act = () => new TenantStreamStoreFactory(null!, Suite.MockTenantStreamDatabaseProvider.Object, Suite.MockEventSerializer.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("configuration");

            // Act
            act = () => new TenantStreamStoreFactory(Suite.Configuration, null!, Suite.MockEventSerializer.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("databaseProvider");

            // Act
            act = () => new TenantStreamStoreFactory(Suite.Configuration, Suite.MockTenantStreamDatabaseProvider.Object, null!);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("serializer");

        }

        [Fact]
        public void When_streamstore_is_created()
        {
            // Arrange
            var databaseProvider = Suite.MockTenantStreamDatabaseProvider;
            databaseProvider.Setup(x => x.GetDatabase(It.IsAny<Id>())).Returns(Generated.MockOf<IStreamDatabase>().Object);
            var factory = new TenantStreamStoreFactory(Suite.Configuration, databaseProvider.Object, Suite.MockEventSerializer.Object);

            // Act
            var store = factory.Create(Generated.Id);

            //Assert
            store.Should().NotBeNull();
        }
    }
}
