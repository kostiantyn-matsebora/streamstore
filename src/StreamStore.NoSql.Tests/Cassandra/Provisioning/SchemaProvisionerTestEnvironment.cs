using Moq;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing.Framework;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning;

public class SchemaProvisionerTestEnvironment: TestEnvironment
{

    internal Mock<ICassandraSessionFactory> SessionFactory { get; }
    internal CassandraSchemaProvisioner SchemaProvisioner => new CassandraSchemaProvisioner(SessionFactory.Object, Testing.Generated.Objects.Single<CassandraStorageConfiguration>());

    public SchemaProvisionerTestEnvironment()
    {
        SessionFactory = MockRepository.Create<ICassandraSessionFactory>();
    }
}
