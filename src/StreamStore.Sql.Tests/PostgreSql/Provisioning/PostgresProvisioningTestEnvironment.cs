using Moq;
using StreamStore.Sql.Multitenancy;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.PostgreSql.Provisioning
{
    public class PostgresProvisioningTestEnvironment : TestEnvironmentBase
    {

        public MockRepository MockRepository = new MockRepository(MockBehavior.Strict);

        public Mock<ISqlTenantStorageConfigurationProvider> MockSqlConfigurationProvider => MockRepository.Create<ISqlTenantStorageConfigurationProvider>();
    }
}
