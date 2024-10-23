using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Sqlite.Configurator
{
    public class SqliteDatabaseConfiguratorTestSuite: TestSuiteBase
    {
        public SqliteDatabaseConfiguratorTestSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            MockServiceCollection = MockRepository.Create<IServiceCollection>();
        }

        public MockRepository MockRepository { get; }
        public Mock<IServiceCollection> MockServiceCollection { get; }
    }
}
