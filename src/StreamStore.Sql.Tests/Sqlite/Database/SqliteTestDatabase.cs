using System.Data.SQLite;
using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public class SqliteTestDatabase: ISqlTestDatabase
    {
        readonly string databaseName;

        public string ConnectionString { get; }

        public SqliteTestDatabase(string databaseName)
        {
            this.databaseName = databaseName.ThrowIfNull(nameof(databaseName));
            ConnectionString = $"Data Source = {databaseName}; Version = 3;";
        }

        public bool EnsureExists()
        {
            SQLiteConnection.CreateFile(databaseName);
            return true;
        }
    }
}
