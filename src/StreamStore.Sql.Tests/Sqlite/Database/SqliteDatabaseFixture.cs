using System.Data.SQLite;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;
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

        protected override IDbConnectionFactory CreateConnectionFactory()
        {
            return new SqliteDbConnectionFactory(CreateConfiguration());
        }

        protected override IDapperCommandFactory CreateCommandFactory()
        {
            return new DefaultDapperCommandFactory(CreateConfiguration());
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new SqliteExceptionHandler();
        }

        SqlDatabaseConfiguration CreateConfiguration()
        {
            return new SqlDatabaseConfigurationBuilder()
                        .WithConnectionString($"Data Source = {DatabaseName}; Version = 3;")
                        .Build();
        }

        protected override void CreateDatabase()
        {
            SQLiteConnection.CreateFile($"{DatabaseName}");
        }

        protected override string GenerateDatabaseName()
        {
            return $"{Generated.String}.sqlite";
        }

        protected override ISqlProvisionQueryProvider CreateProvisionQueryProvider()
        {
           return new SqliteProvisioningQueryProvider(CreateConfiguration());
        }
    }
}
