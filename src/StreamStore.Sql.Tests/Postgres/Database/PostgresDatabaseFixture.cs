
using StreamStore.Sql.API;
using StreamStore.Sql.Postgres;
using StreamStore.Sql.Tests.Database;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Postgres.Database
{
    internal class PostgresDatabaseFixture : SqlDatabaseFixtureBase
    {

        public override string GetConnectionString()
        {
            return String.Empty;
        }
        
        protected override IDbConnectionFactory CreateConnectionFactory()
        {
           return new PostgresConnectionFactory(CreateConfiguration());
        }

        protected override string CreateDatabase()
        {
            return Generated.Id;
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new PostgresExceptionHandler();
        }

        protected override ISqlProvisioningQueryProvider CreateProvisionQueryProvider()
        {
            return new PostgresProvisioningQueryProvider(CreateConfiguration());
        }
    }
}
