using FluentAssertions;
using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.DatabaseProvider;

public class Getting_database: Scenario
{

    [Fact]   
    public void When_getting_database() {
        // Arrange
        var tenant = Generated.Id;
        var tenantMapperProvider = Suite.MockRepository.Create<ICassandraTenantMapperProvider>();
        tenantMapperProvider.Setup(x => x.GetMapperProvider(tenant)).Returns(Suite.MockRepository.Create<ICassandraMapperProvider>().Object);

        var tenantStorageConfigurationProvider = Generated.MockOf<ICassandraTenantStorageConfigurationProvider>();
        tenantStorageConfigurationProvider.Setup(x => x.GetConfiguration(tenant)).Returns(new CassandraStorageConfiguration());

        var cqlQueriesProvider = Generated.MockOf<ICassandraCqlQueriesProvider>();
        cqlQueriesProvider.Setup(x => x.GetCqlQueries(It.IsAny<CassandraStorageConfiguration>())).Returns(Generated.MockOf<ICassandraCqlQueries>().Object);

        var databaseProvider = new CassandraStreamDatabaseProvider(tenantMapperProvider.Object, tenantStorageConfigurationProvider.Object, cqlQueriesProvider.Object);

        // Act
        var database = databaseProvider.GetDatabase(tenant);
        
        // Assert
        database.Should().NotBeNull();
        Suite.MockRepository.VerifyAll();
    }
}
