using System;
using System.Reflection;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public  class CassandraMultitenantConfigurator<TConfigurator> : CassandraConfiguratorBase where TConfigurator : CassandraMultitenantConfigurator<TConfigurator>
    {
        DelegateTenantClusterConfigurator tenantClusterConfigurator = new DelegateTenantClusterConfigurator();
        readonly internal DelegateClusterConfigurator clusterConfigurator = new DelegateClusterConfigurator();
        Type storageConfigurationProviderType = typeof(CassandraStorageConfigurationProvider);
        Type? keyspaceProviderType;


        readonly CassandraKeyspaceRegistry keyspaceProvider = new CassandraKeyspaceRegistry();

        public TConfigurator ConfigureStoragePrototype(Action<CassandraStorageConfigurationBuilder> configure)
        {
            ConfigureStorageInstance(configure);
            return (TConfigurator)this;
        }

        public TConfigurator WithTenantClusterConfigurator(Action<Id, Builder> configurator)
        {
           tenantClusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
           return (TConfigurator)this;
        }

        public TConfigurator WithCredentials(string username, string password)
        {
            clusterConfigurator.AddConfigurator(builder => builder.WithCredentials(username, password));
            return (TConfigurator)this;
        }


        public TConfigurator ConfigureDefaultCluster(Action<Builder> configure)
        {
            clusterConfigurator!.AddConfigurator(configure);
            return (TConfigurator)this;
        }

        public TConfigurator WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
        {
            storageConfigurationProviderType = typeof(TStorageConfigurationProvider);
            return (TConfigurator)this;
        }

        public TConfigurator WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider
        {
            keyspaceProviderType = typeof(TKeyspaceProvider);
            return (TConfigurator)this;
        }

        public TConfigurator AddKeyspace(Id tenantId, string keyspace)
        {
            keyspaceProvider.AddKeyspace(tenantId, keyspace);
            return (TConfigurator)this;
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


    public class CassandraMultitenantConfigurator: CassandraMultitenantConfigurator<CassandraMultitenantConfigurator>
    {
    }
}
