using System.Data.SQLite;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Database;


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

        protected override bool CreateDatabase(out string databaseName)
        {
            databaseName = $"{Generated.DatabaseName}.sqlite";
            SQLiteConnection.CreateFile(databaseName);
            return true;
        }

        string GetConnectionString()
        {
            return $"Data Source = {databaseName}; Version = 3;";
        }
    }
}
