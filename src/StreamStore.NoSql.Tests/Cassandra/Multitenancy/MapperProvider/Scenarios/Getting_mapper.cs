using Cassandra;
using Cassandra.Mapping;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.MapperProvider.Scenarios;

public class Getting_mapper: Scenario
{
    [Fact]
    public void When_getting_mapper() {
        // Arrange
        var tenant = Generated.Id;
        var tenantClusterRegistry = Suite.MockRepository.Create<ICassandraTenantClusterRegistry>();
        tenantClusterRegistry.Setup(x => x.GetCluster(tenant)).Returns(Cluster.Builder().AddContactPoint("localhost").Build());
        var tenantMappingRegistry = Suite.MockRepository.Create<ICassandraTenantMappingRegistry>();
        tenantMappingRegistry.Setup(x => x.GetMapping(tenant)).Returns(new MappingConfiguration());
        var tenantStorageConfigurationProvider = Generated.MockOf<ICassandraTenantStorageConfigurationProvider>();
        tenantStorageConfigurationProvider.Setup(x => x.GetConfiguration(tenant)).Returns(new CassandraStorageConfiguration());
        var tenantMapperProvider = new CassandraTenantMapperProvider(tenantStorageConfigurationProvider.Object, tenantClusterRegistry.Object, tenantMappingRegistry.Object);
        
        // Act
        var mapper = tenantMapperProvider.GetMapperProvider(tenant);
        
        // Assert
        mapper.Should().NotBeNull();
        Suite.MockRepository.VerifyAll();
    }
}
