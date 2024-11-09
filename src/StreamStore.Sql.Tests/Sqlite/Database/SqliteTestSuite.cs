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

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseSqliteDatabase(CreateConfiguration());
        }

        protected override string GetConnectionString()
        {
            return $"Data Source={fixture.DatabaseName};Version=3;";
        }
    }
}
