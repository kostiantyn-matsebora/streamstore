using System;
using System.Data.Common;
using System.Data.SQLite;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Sqlite
{
    internal class SqliteDbConnectionFactory: IDbConnectionFactory
    {
        readonly string connectionString;

        public SqliteDbConnectionFactory(SqlStorageConfiguration configuration): this(configuration.ConnectionString)
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
