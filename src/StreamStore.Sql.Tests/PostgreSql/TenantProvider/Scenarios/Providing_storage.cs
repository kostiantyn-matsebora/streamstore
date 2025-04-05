using FluentAssertions;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Storage;
using StreamStore.Sql.PostgreSql;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.PostgreSql.TenantProvider
{
    public class Providing_storage: Scenario<PostgresTenantProviderTestEnvironment>
    {
        [Fact]
        public void When_any_argument_is_not_degined()
        {
            // Act
            var act = () => new PostgresTenantStorageProvider(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_storage_for_tenant_exists()
        {
            // Arrange
            var configurationProvider = Environment.MockSqlConfigurationProvider;
            var tenantId = Generated.Primitives.Id;
            var provider = new PostgresTenantStorageProvider(configurationProvider.Object);

            configurationProvider
                .Setup(provider => provider.GetConfiguration(tenantId))
                .Returns(new SqlStorageConfiguration());

            // Act
            var storage = provider.GetStorage(tenantId);

            // Assert
            storage.Should().NotBeNull().And.BeOfType<SqlStreamStorage>();
        }
    }
}
