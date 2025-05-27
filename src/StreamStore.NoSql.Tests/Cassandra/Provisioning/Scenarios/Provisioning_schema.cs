using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning;

public class Provisioning_schema : Scenario<SchemaProvisionerTestEnvironment>
{
    [Fact]
    public void When_any_parameter_is_not_set()
    {

        // Act
        var act = () => new CassandraSchemaProvisioner(null!, Generated.Objects.Single<CassandraStorageConfiguration>() );

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraSchemaProvisioner(Environment.SessionFactory.Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    //[Fact]
    //public async Task When_provisioning()
    //{
    //    // Arange
    //    var session = Environment.MockRepository.Create<ISession>();
    //    session.Setup(x => x.Keyspace).Returns(Generated.Primitives.String);
    //    session.Setup(x => x.CreateKeyspaceIfNotExists(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<bool>()));
    //    session.Setup(x => x.Cluster).Returns(Generated.Objects.Single<FakeCluster>());
    //    Environment.SessionFactory.Setup(x => x.Open()).Returns(session.Object);

    //    var provisioner = Environment.SchemaProvisioner;

    //    // Act
    //    await provisioner.ProvisionSchemaAsync(CancellationToken.None);

    //    // Assert
    //    Environment.MockRepository.VerifyAll();
    //}


}
