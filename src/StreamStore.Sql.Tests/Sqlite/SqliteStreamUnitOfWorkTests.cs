using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteStreamUnitOfWorkTests : StreamUnitOfWorkTestsBase
    {
        public SqliteStreamUnitOfWorkTests() : base(new SqliteTestSuite())
        {
        }
    }
}
