using Npgsql;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.Database;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    public sealed class PostgresDatabaseFixture : SqlDatabaseFixtureBase<PostgresTestDatabase>
    {

        public PostgresDatabaseFixture() : base(new PostgresTestDatabase(Generated.Names.Database))
        {
        }
       
        public override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
             configurator.UsePostgresDatabase(
                    c => c.ConfigureDatabase(
                        x => x.WithConnectionString(testDatabase.ConnectionString)));
        }
    }
}
