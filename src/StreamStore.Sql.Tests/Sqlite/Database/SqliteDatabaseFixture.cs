using System.Data.SQLite;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Database;
using StreamStore.SQL;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public sealed class SqliteDatabaseFixture : SqlDatabaseFixtureBase
    {
        readonly string databaseName = $"{Generated.String}.sqlite";

        public string DatabaseName => databaseName;

        public SqliteDatabaseFixture(): base()
        {
        }

        protected override IDbConnectionFactory CreateConnectionFactory()
        {
            return new SqliteDbConnectionFactory(CreateConfiguration());
        }

        protected override IDapperCommandFactory CreateCommandFactory()
        {
            return new SqliteDapperCommandFactory(CreateConfiguration());
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new SqliteExceptionHandler();
        }

        SqlDatabaseConfiguration CreateConfiguration()
        {
            return new SqlDatabaseConfigurationBuilder()
                        .WithConnectionString($"Data Source ={databaseName}; Version = 3;")
                        .Build();
        }

        protected override void CreateDatabase()
        {
            SQLiteConnection.CreateFile($"{databaseName}");
        }
    }
}
