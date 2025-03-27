using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.StorageConfigurationProvider
{
    public class StorageConfigurationProviderTestEnvironment: TestEnvironment
    {

     public Mock<ICassandraKeyspaceProvider> CassandraKeyspaceProvider { get; }


        public StorageConfigurationProviderTestEnvironment()
        {
            CassandraKeyspaceProvider = MockRepository.Create<ICassandraKeyspaceProvider>();
        }
    }
}
