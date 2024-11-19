using System.Collections.Generic;
using System;
using StreamStore.NoSql.Cassandra.API;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraKeyspaceRegistry : ICassandraKeyspaceProvider
    {
        readonly Dictionary<Id, string> keyspaces = new Dictionary<Id, string>();

        public string GetKeyspace(Id tenantId)
        {
            if (!keyspaces.TryGetValue(tenantId, out var connectionString))
            {
                throw new InvalidOperationException($"No keyspace found for tenant {tenantId}");
            }
            return connectionString;
        }

        public ICassandraKeyspaceProvider AddKeyspace(Id tenantId, string connectionString)
        {
            keyspaces.Add(tenantId, connectionString);
            return this;
        }

        public bool Any => keyspaces.Count > 0;
    }
}
