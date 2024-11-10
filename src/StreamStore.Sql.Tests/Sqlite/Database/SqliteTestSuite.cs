using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Database;

namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public class SqliteTestSuite : SqlTestSuiteBase
    {
        public SqliteTestSuite(): this(new SqliteDatabaseFixture())
        {
        }

        public SqliteTestSuite(SqliteDatabaseFixture fixture): base(fixture)
        {
        }
    }
}
