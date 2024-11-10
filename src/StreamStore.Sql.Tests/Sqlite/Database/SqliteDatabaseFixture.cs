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

        public override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseSqliteDatabase(c => c.WithConnectionString(GetConnectionString()));
        }

        protected override string CreateDatabase()
        {
            var databaseName = $"{Generated.String}.sqlite";
            SQLiteConnection.CreateFile(databaseName);
            return databaseName;
        }

        string GetConnectionString()
        {
            return $"Data Source = {databaseName}; Version = 3;";
        }
    }
}
