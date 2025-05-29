using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory
{
    public class ProvisionerFactoryTestEnvironment : TestEnvironment
    {

        internal readonly Mock<ICassandraTenantStorageConfigurationProvider> ConfigurationProvider;
        internal readonly Mock<ICassandraTenantClusterRegistry> ClusterRegistry;

        public ProvisionerFactoryTestEnvironment()
        {
            ConfigurationProvider = MockRepository.Create<ICassandraTenantStorageConfigurationProvider>();
            ClusterRegistry = MockRepository.Create<ICassandraTenantClusterRegistry>();
        }
    }
}
