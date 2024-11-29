using Moq;
using StreamStore.Sql.Multitenancy;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.PostgreSql.Provisioning
{
    public class PostgresProvisioningSuite : TestSuiteBase
    {

        public MockRepository MockRepository = new MockRepository(MockBehavior.Strict);

        public Mock<ISqlTenantDatabaseConfigurationProvider> MockSqlConfigurationProvider => MockRepository.Create<ISqlTenantDatabaseConfigurationProvider>();
    }
}
