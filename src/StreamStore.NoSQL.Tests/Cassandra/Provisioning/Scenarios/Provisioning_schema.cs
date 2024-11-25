using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning;

public class Provisioning_schema : Scenario<SchemaProvisionerSuite>
{
    [Fact]
    public void When_any_parameter_is_not_set()
    {

        // Act
        var act = () => new CassandraSchemaProvisioner(null!, new CassandraStorageConfiguration());

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraSchemaProvisioner(Suite.MapperProvider.Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();

    }

    [Fact]
    public async Task When_provisioning()
    {
        // Arange
        Suite.Mapper.Setup(x => x.ExecuteAsync(It.IsAny<Cql>())).Returns(Task.CompletedTask);

        var provisioner = Suite.SchemaProvisioner;

        // Act
        await provisioner.ProvisionSchemaAsync(CancellationToken.None);

        // Assert
        Suite.MockRepository.VerifyAll();
    }
}
