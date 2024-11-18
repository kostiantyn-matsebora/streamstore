using System;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public  class CassandraMultitenantConfigurator : CassandraConfiguratorBase
    {
        readonly CassandraKeyspaceConfiguration prototype = new CassandraKeyspaceConfiguration();
        Cluster? cluster;
        DelegateTenantClusterConfigurator clusterConfigurator = new DelegateTenantClusterConfigurator();
        Type keyspaceConfigurationProvider = typeof(DefaultCassandraKeyspaceConfigurationProvider);
        Type keyspaceProvider;

        public CassandraMultitenantConfigurator ConfigureKeyspacePrototype(Action<CassandraKeyspaceConfiguration> configure)
        {
           configure(prototype);
           return this;
        }

        public CassandraMultitenantConfigurator WithTenantClusterConfigurator(Action<Id, Builder> configurator)
        {
           clusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
           return this;
        }

        public CassandraMultitenantConfigurator ConfigureDefaultCluster(Action<Builder> configure)
        {
            var builder = Cluster.Builder();
            configure(builder);
            cluster = builder.Build();
            return this;
        }

        public CassandraMultitenantConfigurator WithKeyspaceConfigurationProvider<TKeyspaceConfigurationProvider>() where TKeyspaceConfigurationProvider : ICassandraKeyspaceConfigurationProvider
        {
            keyspaceConfigurationProvider = typeof(TKeyspaceConfigurationProvider);
            return this;
        }

        public CassandraMultitenantConfigurator WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider
        {
            keyspaceProvider = typeof(TKeyspaceProvider);
            return this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
            if (cluster == null) throw new InvalidOperationException("Default cluster not configured");

            if (keyspaceProvider == null && keyspaceConfigurationProvider ==  typeof(DefaultCassandraKeyspaceConfigurationProvider)) 
            {
                throw new InvalidOperationException("Either ICassandraKeyspaceProvider or ICassandraKeyspaceConfigurationProvider must be configured.");
            }

            services.AddSingleton(prototype);
            services.AddSingleton(clusterConfigurator);
            services.AddSingleton(cluster);
            services.AddSingleton(typeof(ICassandraKeyspaceConfigurationProvider), keyspaceConfigurationProvider);
            services.AddSingleton<CassandraTenantClusterRegistry>();
            if (keyspaceProvider != null)
            {
                services.AddSingleton(typeof(ICassandraKeyspaceProvider), keyspaceProvider);
            }
        }
    }
}
