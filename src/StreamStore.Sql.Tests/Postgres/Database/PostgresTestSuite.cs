using StreamStore.Sql.Postgres;
using StreamStore.Sql.Tests.Database;

namespace StreamStore.Sql.Tests.Postgres.Database
{
    public class PostgresTestSuite : SqlTestSuiteBase
    {
        public PostgresTestSuite(): base(new PostgresDatabaseFixture())
        {
        }
    }
}
