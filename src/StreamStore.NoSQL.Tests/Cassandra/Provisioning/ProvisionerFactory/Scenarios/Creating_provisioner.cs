using Cassandra;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory;
public class Creating_provisioner : Scenario<ProvisionerFactorySuite>
{

    [Fact]
    public void When_provisioner_created()
    {
        // Arrange
        var tenantId = Generated.Id;
        Suite.ConfigurationProvider.Setup(x => x.GetStorageConfiguration(tenantId)).Returns(new CassandraStorageConfiguration());
        Suite.TenantClusterRegistry.Setup(x => x.GetCluster(tenantId)).Returns(Cluster.Builder().AddContactPoint("localhost").Build());
        var provisionerFactory = new CassandraSchemaProvisionerFactory(Suite.ConfigurationProvider.Object, Suite.PredicateProvider.Object, Suite.TenantClusterRegistry.Object);

        // Act
        var provisioner = provisionerFactory.Create(tenantId);

        // Assert
        Suite.MockRepository.VerifyAll();
        provisioner.Should().NotBeNull();
    }
}

