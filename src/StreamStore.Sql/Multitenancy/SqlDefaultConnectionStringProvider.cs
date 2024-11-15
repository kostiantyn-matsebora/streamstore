using System;
using System.Collections.Generic;

namespace StreamStore.Sql.Multitenancy
{
    class SqlDefaultConnectionStringProvider : ISqlTenantConnectionStringProvider
    {
        readonly Dictionary<Id, string> connectionStrings = new Dictionary<Id, string>();

        public string GetConnectionString(Id tenantId)
        {
            if (!connectionStrings.TryGetValue(tenantId, out var connectionString))
            {
                throw new InvalidOperationException($"No connection string found for tenant {tenantId}");
            }
            return connectionString;
        }

        public ISqlTenantConnectionStringProvider AddConnectionString(Id tenantId, string connectionString)
        {
           connectionStrings.Add(tenantId, connectionString);
           return this;
        }

        public bool Any => connectionStrings.Count > 0;
    }
}
