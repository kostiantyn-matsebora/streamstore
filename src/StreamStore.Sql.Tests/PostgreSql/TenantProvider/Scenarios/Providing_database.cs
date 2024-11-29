using FluentAssertions;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;
using StreamStore.Sql.PostgreSql;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.PostgreSql.TenantProvider
{
    public class Providing_database: Scenario<PostgresTenantProviderSuite>
    {
        [Fact]
        public void When_any_argument_is_not_degined()
        {
            // Act
            var act = () => new PostgresTenantDatabaseProvider(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_database_for_tenant_exists()
        {
            // Arrange
            var configurationProvider = Suite.MockSqlConfigurationProvider;
            var tenantId = Generated.Id;
            var provider = new PostgresTenantDatabaseProvider(configurationProvider.Object);

            configurationProvider
                .Setup(provider => provider.GetConfiguration(tenantId))
                .Returns(new SqlDatabaseConfiguration());

            // Act
            var database = provider.GetDatabase(tenantId);

            // Assert
            database.Should().NotBeNull().And.BeOfType<SqlStreamDatabase>();
        }
    }
}
