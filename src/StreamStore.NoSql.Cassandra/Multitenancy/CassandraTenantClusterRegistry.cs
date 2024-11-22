using System.Collections.Concurrent;
using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraTenantClusterRegistry
    {

        readonly IClusterConfigurator clusterConfigurator;
        readonly ITenantClusterConfigurator tenantConfigurator;
        readonly ConcurrentDictionary<Id, Cluster> clusters = new ConcurrentDictionary<Id, Cluster>();

        public CassandraTenantClusterRegistry(IClusterConfigurator clusterConfigurator, ITenantClusterConfigurator tenantConfigurator)
        {
            this.clusterConfigurator = clusterConfigurator.ThrowIfNull(nameof(clusterConfigurator));
            this.tenantConfigurator = tenantConfigurator.ThrowIfNull(nameof(tenantConfigurator));
        }

        public Cluster GetCluster(Id tenantId)
        {
            return clusters.GetOrAdd(tenantId, CreateCluster);
        }

        Cluster CreateCluster(Id tenantId)
        {
            var builder = Cluster.Builder();
            clusterConfigurator.Configure(builder);
            tenantConfigurator.Configure(tenantId, builder);

            return builder.Build();
        }
    }
}
