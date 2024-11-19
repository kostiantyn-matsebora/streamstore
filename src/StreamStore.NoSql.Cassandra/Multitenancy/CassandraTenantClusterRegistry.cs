using System.Collections.Concurrent;
using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraTenantClusterRegistry
    {
        readonly Builder builder;
        readonly DelegateTenantClusterConfigurator configurator;
        readonly ConcurrentDictionary<Id, Cluster> clusters = new ConcurrentDictionary<Id, Cluster>();

        public CassandraTenantClusterRegistry(Builder builder, DelegateTenantClusterConfigurator configurator)
        {
            this.builder = builder.ThrowIfNull(nameof(builder));
            this.configurator = configurator.ThrowIfNull(nameof(configurator));
        }

        public Cluster GetCluster(Id tenantId)
        {
            return clusters.GetOrAdd(tenantId, CreateCluster);
        }

        private Cluster CreateCluster(Id tenantId)
        {
            configurator.Configure(tenantId, builder);
            return builder.Build();
        }
    }
}
