using System;
using System.Data.Common;
using System.Data.SQLite;
using StreamStore.Sql;


namespace StreamStore.SQL.Sqlite
{
    internal class SqliteDbConnectionFactory: IDbConnectionFactory
    {
        readonly string connectionString;

        public SqliteDbConnectionFactory(SqlDatabaseConfiguration configuration): this(configuration.ConnectionString)
        {
        }
        
        public SqliteDbConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString.ThrowIfNull(nameof(connectionString));
        }

        public DbConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
