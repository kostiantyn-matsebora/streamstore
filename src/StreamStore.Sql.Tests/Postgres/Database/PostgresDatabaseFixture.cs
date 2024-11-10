using StreamStore.Sql.Postgres;
using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.Postgres.Database
{
    internal class PostgresDatabaseFixture : SqlDatabaseFixtureBase
    {
        public override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
           configurator.UsePostgresDatabase(c => c.WithConnectionString(GetConnectionString()));
        }
        
        protected override string CreateDatabase()
        {
            throw new NotImplementedException();
        }

        string GetConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
