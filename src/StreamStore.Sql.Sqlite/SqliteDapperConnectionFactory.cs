using System;
using System.Data.Common;
using System.Data.SQLite;
using StreamStore.Sql;

namespace StreamStore.SQL.Sqlite
{
    internal class SqliteDapperConnectionFactory: IDbConnectionFactory
    {
        readonly SqlDatabaseConfiguration configuration;

        public SqliteDapperConnectionFactory(SqlDatabaseConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbConnection GetConnection()
        {
           return new SQLiteConnection(configuration.ConnectionString);
        }
    }
}
