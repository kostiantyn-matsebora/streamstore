using Npgsql;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    public sealed class PostgresDatabaseFixture : SqlDatabaseFixtureBase
    {

        public PostgresDatabaseFixture() : base(new PostgresTestDatabase(Generated.DatabaseName))
        {
        }
       
        public override void ConfigureDatabase(ISingleTenantDatabaseConfigurator configurator)
        {
             configurator.UsePostgresDatabase(
                    c => c.ConfigureDatabase(
                        x => x.WithConnectionString(connectionString)));
            
        }
    }
}
