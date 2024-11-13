using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public sealed class SqliteDatabaseFixture : SqlDatabaseFixtureBase
    {
        public SqliteDatabaseFixture(): base(new SqliteTestDatabase($"{Generated.DatabaseName}.sqlite"))
        {
        }

        public override void ConfigureDatabase(ISingleTenantDatabaseConfigurator configurator)
        {
               configurator.UseSqliteDatabase(
                    c => c.ConfigureDatabase(
                        x => x.WithConnectionString(connectionString)));
        }
    }
}
