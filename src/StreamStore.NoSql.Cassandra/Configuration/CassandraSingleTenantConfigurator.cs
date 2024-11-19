using System;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraSingleTenantConfigurator: CassandraConfiguratorBase
    {
        Cluster? cluster;

        public CassandraSingleTenantConfigurator ConfigureCluster(Action<Builder> configure)
        {
           var builder = Cluster.Builder();
           configure(builder);
           cluster = builder.Build();
           return this;
        }

        public CassandraConfiguratorBase ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
           ConfigureStorageInstance(configure);
           return this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
          if (cluster == null) throw new InvalidOperationException("Cluster not configured");
          services.AddSingleton(cluster);
        }
    }
}
