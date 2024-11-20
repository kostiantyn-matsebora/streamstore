﻿using System;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public  class CassandraMultitenantConfigurator : CassandraConfiguratorBase
    {
        Builder? builder;
        DelegateTenantClusterConfigurator clusterConfigurator = new DelegateTenantClusterConfigurator();
        Type storageConfigurationProviderType = typeof(DefaultCassandraStorageConfigurationProvider);
        Type? keyspaceProviderType;


        readonly CassandraKeyspaceRegistry keyspaceProvider = new CassandraKeyspaceRegistry();

        public CassandraMultitenantConfigurator ConfigureStoragePrototype(Action<CassandraStorageConfigurationBuilder> configure)
        {
            ConfigureStorageInstance(configure);
            return this;
        }

        public CassandraMultitenantConfigurator WithTenantClusterConfigurator(Action<Id, Builder> configurator)
        {
           clusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
           return this;
        }

        public CassandraMultitenantConfigurator ConfigureDefaultCluster(Action<Builder> configure)
        {
            builder = Cluster.Builder();
            configure(builder);
            return this;
        }

        public CassandraMultitenantConfigurator WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraStorageConfigurationProvider
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


        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
            if (builder == null) throw new InvalidOperationException("Default cluster not configured");

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

            services.AddSingleton(typeof(ICassandraStorageConfigurationProvider), storageConfigurationProviderType);
            services.AddSingleton(clusterConfigurator);
            services.AddSingleton(builder);
            services.AddSingleton<CassandraTenantClusterRegistry>();
        }
    }
}