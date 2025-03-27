using Cassandra;
using Cassandra.Mapping;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory;
public class Creating_provisioner : Scenario<ProvisionerFactoryTestEnvironment>
{

    [Fact]
    public void When_provisioner_created()
    {
        // Arrange
        var tenantId = Generated.Primitives.Id;
        Environment.ConfigurationProvider.Setup(x => x.GetConfiguration(tenantId)).Returns(new CassandraStorageConfiguration());
        Environment.TenantMapperProvider.Setup(x => x.GetMapperProvider(tenantId)).Returns(Environment.MockRepository.Create<ICassandraMapperProvider>().Object);
        var provisionerFactory = new CassandraSchemaProvisionerFactory(Environment.ConfigurationProvider.Object, Environment.TenantMapperProvider.Object);

        // Act
        var provisioner = provisionerFactory.Create(tenantId);

        // Assert
        Environment.MockRepository.VerifyAll();
        provisioner.Should().NotBeNull();
    }
}

