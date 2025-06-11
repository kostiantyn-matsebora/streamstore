using StreamStore.Sql.Tests.Storage;

namespace StreamStore.Sql.Tests.Sqlite.Storage
{
    public class SqliteTestEnvironment : SqlTestEnvironmentBase<SqliteTestStorage>
    {
        public SqliteTestEnvironment() : this(new SqliteStorageFixture())
        {
        }

        public SqliteTestEnvironment(SqliteStorageFixture fixture) : base(fixture)
        {
        }
    }
}
