using System;
using System.Data.Common;
using System.Data.SQLite;


namespace StreamStore.SQL.Sqlite
{
    internal class SqliteDbConnectionFactory: IDbConnectionFactory
    {
        readonly SqliteDatabaseConfiguration configuration;

        public SqliteDbConnectionFactory(SqliteDatabaseConfiguration configuration)
        {
          this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbConnection GetConnection()
        {
            return new SQLiteConnection(configuration.ConnectionString);
        }
    }
}
