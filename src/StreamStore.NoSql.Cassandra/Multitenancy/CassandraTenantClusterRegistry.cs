﻿using System.Collections.Concurrent;
using Cassandra;
using StreamStore.Extensions;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraTenantClusterRegistry : ICassandraTenantClusterRegistry
    {

        readonly IClusterConfigurator clusterConfigurator;
        readonly ITenantClusterConfigurator tenantConfigurator;
        readonly ConcurrentDictionary<Id, ICluster> clusters = new ConcurrentDictionary<Id, ICluster>();

        public CassandraTenantClusterRegistry(IClusterConfigurator clusterConfigurator, ITenantClusterConfigurator tenantConfigurator)
        {
            this.clusterConfigurator = clusterConfigurator.ThrowIfNull(nameof(clusterConfigurator));
            this.tenantConfigurator = tenantConfigurator.ThrowIfNull(nameof(tenantConfigurator));
        }

        public ICluster GetCluster(Id tenantId)
        {
            return clusters.GetOrAdd(tenantId, CreateCluster);
        }

        ICluster CreateCluster(Id tenantId)
        {
            var builder = Cluster.Builder();
            clusterConfigurator.Configure(builder);
            tenantConfigurator.Configure(tenantId, builder);

            return builder.Build();
        }
    }
}
