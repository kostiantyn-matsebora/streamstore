using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.StorageProvider;

public class Instantiating : Scenario
{
    [Fact]
    public void When_any_argument_is_not_set()
    {
        // Act
        var act = () => new CassandraStreamStorageProvider(Generated.Mocks.Single<ICassandraTenantMapperProvider>().Object, null!, Generated.Mocks.Single<ICassandraCqlQueriesProvider>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraStreamStorageProvider(null!, Generated.Mocks.Single<ICassandraTenantStorageConfigurationProvider>().Object, Generated.Mocks.Single<ICassandraCqlQueriesProvider>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraStreamStorageProvider(Generated.Mocks.Single<ICassandraTenantMapperProvider>().Object, Generated.Mocks.Single<ICassandraTenantStorageConfigurationProvider>().Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
