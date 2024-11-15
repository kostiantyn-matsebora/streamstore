using FluentAssertions;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;
using StreamStore.Sql.PostgreSql;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.PostgreSql.TenantProvider
{
    public class When_providing_database: Scenario<PostgresTenantProviderSuite>
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
        public void When_providing_postgres_database()
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
