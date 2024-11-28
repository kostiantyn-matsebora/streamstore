using System;
using System.Reflection;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Extensions;
using StreamStore.NoSql.Cassandra.Multitenancy;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public  class CassandraMultitenantConfigurator : CassandraConfiguratorBase
    {
        DelegateTenantClusterConfigurator tenantClusterConfigurator = new DelegateTenantClusterConfigurator();
        readonly internal DelegateClusterConfigurator clusterConfigurator = new DelegateClusterConfigurator();
        Type storageConfigurationProviderType = typeof(CassandraStorageConfigurationProvider);
        Type? keyspaceProviderType;


        readonly CassandraKeyspaceRegistry keyspaceProvider = new CassandraKeyspaceRegistry();

        public CassandraMultitenantConfigurator ConfigureStoragePrototype(Action<CassandraStorageConfigurationBuilder> configure)
        {
            ConfigureStorageInstance(configure);
            return this;
        }

        public CassandraMultitenantConfigurator WithTenantClusterConfigurator(Action<Id, Builder> configurator)
        {
           tenantClusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
           return this;
        }

        public CassandraMultitenantConfigurator WithCredentials(string username, string password)
        {
            clusterConfigurator.AddConfigurator(builder => builder.WithCredentials(username, password));
            return this;
        }


        public CassandraMultitenantConfigurator ConfigureDefaultCluster(Action<Builder> configure)
        {
            clusterConfigurator!.AddConfigurator(configure);
            return this;
        }

        public CassandraMultitenantConfigurator WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
        {
            storageConfigurationProviderType = typeof(TStorageConfigurationProvider);
            return this;
        }

        public CassandraMultitenantConfigurator WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider
        {
            keyspaceProviderType = typeof(TKeyspaceProvider);
            return this;
        }

        public CassandraMultitenantConfigurator AddKeyspace(Id tenantId, string keyspace)
        {
            keyspaceProvider.AddKeyspace(tenantId, keyspace);
            return this;
        }

        public CassandraMultitenantConfigurator UseCosmosDb()
        {
            mode = CassandraMode.CosmosDbCassandra;
            return this;
        }

        public CassandraMultitenantConfigurator UseAppConfig(IConfiguration configuration, string connectionStringName = "StreamStore")
        {
            clusterConfigurator.AddConfigurator(builder => builder.UseAppConfig(configuration, connectionStringName));
            return this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
            if (keyspaceProviderType != null)
            {
                services.AddSingleton(typeof(ICassandraKeyspaceProvider), keyspaceProviderType);
            }
            else
            {
                if (!keyspaceProvider.Any)
                {
                    throw new InvalidOperationException("Neither ICassandraKeyspaceProvider nor tenant keyspaces provided.");
                }
                services.AddSingleton(typeof(ICassandraKeyspaceProvider), keyspaceProvider);
            }

            services.AddSingleton(typeof(ICassandraTenantStorageConfigurationProvider), storageConfigurationProviderType);
            services.AddSingleton(typeof(ICassandraTenantMapperProvider), typeof(CassandraTenantMapperProvider));
            services.AddSingleton(typeof(ITenantClusterConfigurator), tenantClusterConfigurator);
            services.AddSingleton(typeof(IClusterConfigurator), clusterConfigurator);
            services.AddSingleton<ICassandraTenantClusterRegistry, CassandraTenantClusterRegistry>();
            services.AddSingleton<ICassandraTenantMappingRegistry, CassandraTenantMappingRegistry>();
            services.AddSingleton<ICassandraCqlQueriesProvider>(new CassandraCqlQueriesProvider(mode));
        }
    }

}
