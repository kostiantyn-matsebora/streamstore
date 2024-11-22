using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory
{
    public class ProvisionerFactorySuite : TestSuite
    {

        internal readonly Mock<ICassandraStorageConfigurationProvider> ConfigurationProvider;
        internal readonly Mock<ICassandraTenantClusterRegistry> TenantClusterRegistry;
        internal readonly Mock<ICassandraPredicateProvider> PredicateProvider;

        public ProvisionerFactorySuite()
        {
            TenantClusterRegistry = MockRepository.Create<ICassandraTenantClusterRegistry>();
            ConfigurationProvider = MockRepository.Create<ICassandraStorageConfigurationProvider>();
            PredicateProvider = MockRepository.Create<ICassandraPredicateProvider>();
        }
    }
}
