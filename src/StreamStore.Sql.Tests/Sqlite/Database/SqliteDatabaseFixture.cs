using System.Data.SQLite;
using StreamStore.Sql.API;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Database;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public sealed class SqliteDatabaseFixture : SqlDatabaseFixtureBase
    {
        public SqliteDatabaseFixture(): base()
        {
        }

        public override string GetConnectionString()
        {
            return $"Data Source = {databaseName}; Version = 3;";
        }

        protected override IDbConnectionFactory CreateConnectionFactory()
        {
            return new SqliteDbConnectionFactory(CreateConfiguration());
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new SqliteExceptionHandler();
        }

        protected override string CreateDatabase()
        {
            var databaseName = $"{Generated.String}.sqlite";
            SQLiteConnection.CreateFile(databaseName);
            return databaseName;
        }

        protected override ISqlProvisioningQueryProvider CreateProvisionQueryProvider()
        {
           return new SqliteProvisioningQueryProvider(CreateConfiguration());
        }
    }
}
