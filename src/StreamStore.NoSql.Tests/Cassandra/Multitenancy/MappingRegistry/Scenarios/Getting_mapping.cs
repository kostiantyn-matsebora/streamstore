using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.MappingRegistry.Scenarios;

public class Getting_mapping: Scenario
{
    [Fact]
    public void When_getting_mapping() {
        // Arrange

        var tenantId = Generated.Id;
        var configurationProvider = Suite.MockRepository.Create<ICassandraTenantStorageConfigurationProvider>();
        configurationProvider.Setup(x => x.GetConfiguration(tenantId)).Returns(new CassandraStorageConfiguration());
        var registry = new CassandraTenantMappingRegistry(configurationProvider.Object);

        // Act
        var result = registry.GetMapping(tenantId);

        // Assert
        result.Should().NotBeNull();
        Suite.MockRepository.VerifyAll();

        // Act
        var second_result = registry.GetMapping(tenantId);

        // Assert
        second_result.Should().BeSameAs(result);
    }
}
