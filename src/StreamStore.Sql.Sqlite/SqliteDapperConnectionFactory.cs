using System;
using System.Data.Common;
using System.Data.SQLite;

namespace StreamStore.SQL.Sqlite
{
    internal class SqliteDapperConnectionFactory: IDbConnectionFactory
    {
        readonly SqliteDatabaseConfiguration configuration;

        public SqliteDapperConnectionFactory(SqliteDatabaseConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbConnection GetConnection()
        {
           return new SQLiteConnection(configuration.ConnectionString);
        }
    }
}
