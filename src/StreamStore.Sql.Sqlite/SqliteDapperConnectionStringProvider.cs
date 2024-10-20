using System;
using Dapper.Extensions;

namespace StreamStore.SQL.Sqlite
{
    internal class SqliteDapperConnectionStringProvider : IConnectionStringProvider
    {
        readonly SqliteDatabaseConfiguration configuration;

        public SqliteDapperConnectionStringProvider(SqliteDatabaseConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public string GetConnectionString(string connectionName, bool enableMasterSlave = false, bool readOnly = false)
        {
            return configuration.ConnectionString;
        }
    }
}
