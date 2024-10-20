using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteStreamDatabaseTests : StreamDatabaseTestsBase
    {
        public SqliteStreamDatabaseTests()
            : base(new SqliteTestSuite())
        {
        }
    }
}