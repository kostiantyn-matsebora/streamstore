using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteStreamDatabaseTests : StreamDatabaseTestsBase<SqliteTestSuite>
    {
        public SqliteStreamDatabaseTests()
            : base(new SqliteTestSuite())
        {
        }
    }
}