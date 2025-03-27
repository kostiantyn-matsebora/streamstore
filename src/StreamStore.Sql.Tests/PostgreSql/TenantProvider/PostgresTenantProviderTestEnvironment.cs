using Moq;
using StreamStore.Sql.Multitenancy;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.PostgreSql.TenantProvider
{
    public class PostgresTenantProviderTestEnvironment: TestEnvironmentBase
    {
        public readonly MockRepository MockRepository = new MockRepository(MockBehavior.Strict);

        public Mock<ISqlTenantDatabaseConfigurationProvider> MockSqlConfigurationProvider => MockRepository.Create<ISqlTenantDatabaseConfigurationProvider>();
    }
}
