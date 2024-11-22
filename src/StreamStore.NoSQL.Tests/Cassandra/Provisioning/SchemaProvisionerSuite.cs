using Moq;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning;

public class SchemaProvisionerSuite: TestSuite
{

    internal Mock<ICassandraStreamRepositoryFactory> RepositoryFactory { get; }

    public SchemaProvisionerSuite()
    {
        RepositoryFactory = MockRepository.Create<ICassandraStreamRepositoryFactory>();

    }
}
