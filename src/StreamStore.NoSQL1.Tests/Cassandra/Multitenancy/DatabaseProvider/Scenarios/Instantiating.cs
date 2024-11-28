using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.DatabaseProvider;

public class Instantiating : Scenario
{
    [Fact]
    public void When_any_argument_is_not_set()
    {
        // Act
        var act = () => new CassandraStreamDatabaseProvider(Generated.MockOf<ICassandraTenantMapperProvider>().Object, null!, Generated.MockOf<ICassandraCqlQueriesProvider>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraStreamDatabaseProvider(null!, Generated.MockOf<ICassandraTenantStorageConfigurationProvider>().Object, Generated.MockOf<ICassandraCqlQueriesProvider>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraStreamDatabaseProvider(Generated.MockOf<ICassandraTenantMapperProvider>().Object, Generated.MockOf<ICassandraTenantStorageConfigurationProvider>().Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
