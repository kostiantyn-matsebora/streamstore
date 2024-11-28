using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.StorageConfigurationProvider
{
    public class StorageConfigurationProviderSuite: TestSuite
    {

     public Mock<ICassandraKeyspaceProvider> CassandraKeyspaceProvider { get; }


        public StorageConfigurationProviderSuite()
        {
            CassandraKeyspaceProvider = MockRepository.Create<ICassandraKeyspaceProvider>();
        }
    }
}
