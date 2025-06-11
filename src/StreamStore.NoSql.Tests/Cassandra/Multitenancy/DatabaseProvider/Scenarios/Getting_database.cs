using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.StorageProvider;

public class Getting_storage: Scenario
{

    [Fact]   
    public void When_getting_storage() {
        // Arrange
        var tenant = Generated.Primitives.Id;
        var tenantMapperProvider = Environment.MockRepository.Create<ICassandraTenantMapperProvider>();
        var mapperProvider = Environment.MockRepository.Create<ICassandraMapperProvider>();
        tenantMapperProvider.Setup(x => x.GetMapperProvider(tenant)).Returns(mapperProvider.Object);
        
        var tenantStorageConfigurationProvider = Generated.Mocks.Single<ICassandraTenantStorageConfigurationProvider>();
        tenantStorageConfigurationProvider.Setup(x => x.GetConfiguration(tenant)).Returns(new CassandraStorageConfiguration());

        var cqlQueriesProvider = Generated.Mocks.Single<ICassandraCqlQueriesProvider>();
        cqlQueriesProvider.Setup(x => x.GetCqlQueries(It.IsAny<CassandraStorageConfiguration>())).Returns(Generated.Mocks.Single<ICassandraCqlQueries>().Object);

        var storageProvider = new CassandraStreamStorageProvider(tenantMapperProvider.Object, tenantStorageConfigurationProvider.Object, cqlQueriesProvider.Object);

        // Act
        var storage = storageProvider.GetStorage(tenant);
        
        // Assert
        storage.Should().NotBeNull();
        Environment.MockRepository.VerifyAll();
    }
}
