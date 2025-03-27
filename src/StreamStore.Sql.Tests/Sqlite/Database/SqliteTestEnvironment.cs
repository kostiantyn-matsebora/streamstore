using StreamStore.Sql.Tests.Database;

namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public class SqliteTestEnvironment : SqlTestEnvironmentBase<SqliteTestDatabase>
    {
        public SqliteTestEnvironment(): this(new SqliteDatabaseFixture())
        {
        }

        public SqliteTestEnvironment(SqliteDatabaseFixture fixture): base(fixture)
        {
        }
    }
}
