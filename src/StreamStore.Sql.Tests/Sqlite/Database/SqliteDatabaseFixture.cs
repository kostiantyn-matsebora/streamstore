using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Database;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public sealed class SqliteDatabaseFixture : SqlDatabaseFixtureBase<SqliteTestDatabase>
    {
        public SqliteDatabaseFixture(): base(new SqliteTestDatabase($"{Generated.Names.Database}.sqlite"))
        {
        }

        public override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
               configurator.UseSqliteDatabase(
                    c => c.ConfigureDatabase(
                        x => x.WithConnectionString(testDatabase.ConnectionString)));
        }
    }
}
