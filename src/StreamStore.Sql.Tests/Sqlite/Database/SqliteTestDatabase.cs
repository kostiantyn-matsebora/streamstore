using System.Data.SQLite;
using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public sealed class SqliteTestDatabase: ISqlTestDatabase
    {
        readonly string databaseName;
        private bool disposedValue;

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

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    File.Delete(databaseName);
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
