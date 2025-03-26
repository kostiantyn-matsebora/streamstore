using Cassandra;
using Cassandra.Mapping;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory;
public class Creating_provisioner : Scenario<ProvisionerFactorySuite>
{

    [Fact]
    public void When_provisioner_created()
    {
        // Arrange
        var tenantId = Generated.Primitives.Id;
        Suite.ConfigurationProvider.Setup(x => x.GetConfiguration(tenantId)).Returns(new CassandraStorageConfiguration());
        Suite.TenantMapperProvider.Setup(x => x.GetMapperProvider(tenantId)).Returns(Suite.MockRepository.Create<ICassandraMapperProvider>().Object);
        var provisionerFactory = new CassandraSchemaProvisionerFactory(Suite.ConfigurationProvider.Object, Suite.TenantMapperProvider.Object);

        // Act
        var provisioner = provisionerFactory.Create(tenantId);

        // Assert
        Suite.MockRepository.VerifyAll();
        provisioner.Should().NotBeNull();
    }
}

