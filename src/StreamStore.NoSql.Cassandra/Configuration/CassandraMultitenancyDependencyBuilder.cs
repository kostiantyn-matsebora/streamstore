using System;
using System.Linq;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraMultitenancyDependencyBuilder
    {

        DelegateTenantClusterConfigurator tenantClusterConfigurator = new DelegateTenantClusterConfigurator();
        readonly CassandraKeyspaceRegistry keyspaceProvider = new CassandraKeyspaceRegistry();
        IServiceCollection services = new ServiceCollection();

        public CassandraMultitenancyDependencyBuilder()
        {
            services.AddSingleton<ITenantClusterConfigurator>(tenantClusterConfigurator);
            services.AddSingleton<ICassandraTenantStorageConfigurationProvider, CassandraStorageConfigurationProvider>();
            services.AddSingleton<ICassandraKeyspaceProvider>(keyspaceProvider);
        }

        public CassandraMultitenancyDependencyBuilder WithTenantClusterConfigurator(Action<Id, Builder> configurator)
        {
            tenantClusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
            services.AddSingleton<ITenantClusterConfigurator>(tenantClusterConfigurator);
            return this;
        }

        public CassandraMultitenancyDependencyBuilder WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
        {
            services.AddSingleton(typeof(ICassandraTenantStorageConfigurationProvider), typeof(TStorageConfigurationProvider));
            return this;
        }


        public CassandraMultitenancyDependencyBuilder WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider
        {
            services.AddSingleton(typeof(ICassandraKeyspaceProvider), typeof(TKeyspaceProvider));
            return this;
        }

        public CassandraMultitenancyDependencyBuilder AddKeyspace(Id tenantId, string keyspace)
        {
            keyspaceProvider.AddKeyspace(tenantId, keyspace);
            return this;
        }

        public IServiceCollection Build()
        {
            return services;
        }
    }
}
