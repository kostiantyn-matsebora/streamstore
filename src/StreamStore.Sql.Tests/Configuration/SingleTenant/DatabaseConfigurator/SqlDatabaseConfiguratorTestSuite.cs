using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Configuration.SingleTenant.DatabaseConfigurator
{
    public class SqlDatabaseConfiguratorTestSuite : TestSuiteBase
    {
        public SqlDatabaseConfiguratorTestSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            MockServiceCollection = MockRepository.Create<IServiceCollection>();
        }

        public MockRepository MockRepository { get; }
        public Mock<IServiceCollection> MockServiceCollection { get; }
    }
}
