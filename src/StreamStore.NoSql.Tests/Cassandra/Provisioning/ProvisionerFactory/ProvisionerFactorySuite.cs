using Moq;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory
{
    public class ProvisionerFactorySuite : TestSuite
    {

        internal readonly Mock<ICassandraTenantStorageConfigurationProvider> ConfigurationProvider;
        internal readonly Mock<ICassandraTenantMapperProvider> TenantMapperProvider;

        public ProvisionerFactorySuite()
        {
            TenantMapperProvider = MockRepository.Create<ICassandraTenantMapperProvider>();
            ConfigurationProvider = MockRepository.Create<ICassandraTenantStorageConfigurationProvider>();
        }
    }
}
