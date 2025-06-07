//using System;
//using System.Net.Security;
//using Cassandra;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.NoSql.Cassandra.API;
//using StreamStore.NoSql.Cassandra.Storage;
//using StreamStore.NoSql.Cassandra.Extensions;
//using StreamStore.NoSql.Cassandra.Multitenancy;

//namespace StreamStore.NoSql.Cassandra.Configuration
//{
//    public  class CassandraMultitenantConfigurator : CassandraConfiguratorBase
//    {
//        DelegateTenantClusterConfigurator tenantClusterConfigurator = new DelegateTenantClusterConfigurator();
//        readonly internal DelegateClusterConfigurator clusterConfigurator = new DelegateClusterConfigurator();
//        Type storageConfigurationProviderType = typeof(CassandraStorageConfigurationProvider);
//        Type? keyspaceProviderType;


//        readonly CassandraKeyspaceRegistry keyspaceProvider = new CassandraKeyspaceRegistry();

//        public CassandraMultitenantConfigurator ConfigureStoragePrototype(Action<CassandraStorageConfigurationBuilder> configure)
//        {
//            ConfigureStorageInstance(configure);
//            return this;
//        }

//        public CassandraMultitenantConfigurator WithTenantClusterConfigurator(Action<Id, Builder> configurator)
//        {
//           tenantClusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
//           return this;
//        }

//        public CassandraMultitenantConfigurator ConfigureDefaultCluster(Action<Builder> configure)
//        {
//            clusterConfigurator!.AddConfigurator(configure);
//            return this;
//        }

//        public CassandraMultitenantConfigurator WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
//        {
//            storageConfigurationProviderType = typeof(TStorageConfigurationProvider);
//            return this;
//        }

//        public CassandraMultitenantConfigurator WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider
//        {
//            keyspaceProviderType = typeof(TKeyspaceProvider);
//            return this;
//        }

//        public CassandraMultitenantConfigurator AddKeyspace(Id tenantId, string keyspace)
//        {
//            keyspaceProvider.AddKeyspace(tenantId, keyspace);
//            return this;
//        }

//        public CassandraMultitenantConfigurator UseCosmosDb(IConfiguration configuration, string connectionStringName = "StreamStore", RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
//        {
//            var connectionString = configuration.GetConnectionString(connectionStringName);
//            if (string.IsNullOrEmpty(connectionString))
//            {
//                throw new ArgumentException($"Connection string {connectionStringName} is not found in configuration", nameof(configuration));
//            }

//            return UseCosmosDb(connectionString, remoteCertValidationCallback);
//        }

//        public CassandraMultitenantConfigurator UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
//        {
//            mode = CassandraMode.CosmosDbCassandra;
//            if (connectionString != null)
//            {
//                clusterConfigurator.AddConfigurator(builder => builder.WithCosmosDbConnectionString(connectionString, remoteCertValidationCallback));
//            }
//            return this;
//        }

//        protected override void ApplySpecificDependencies(IServiceCollection services)
//        {
//            if (keyspaceProviderType != null)
//            {
//                services.AddSingleton(typeof(ICassandraKeyspaceProvider), keyspaceProviderType);
//            }
//            else
//            {
//                if (!keyspaceProvider.Any)
//                {
//                    throw new InvalidOperationException("Neither ICassandraKeyspaceProvider nor tenant keyspaces provided.");
//                }
//                services.AddSingleton(typeof(ICassandraKeyspaceProvider), keyspaceProvider);
//            }

//            services.AddSingleton(typeof(ICassandraTenantStorageConfigurationProvider), storageConfigurationProviderType);
//            services.AddSingleton(typeof(ICassandraTenantMapperProvider), typeof(CassandraTenantMapperProvider));
//            services.AddSingleton(typeof(ITenantClusterConfigurator), tenantClusterConfigurator);
//            services.AddSingleton(typeof(IClusterConfigurator), clusterConfigurator);
//            services.AddSingleton<ICassandraTenantClusterRegistry, CassandraTenantClusterRegistry>();
//            services.AddSingleton<ICassandraTenantMappingRegistry, CassandraTenantMappingRegistry>();
//            services.AddSingleton<ICassandraCqlQueriesProvider>(new CassandraCqlQueriesProvider(mode));
//        }
//    }

//}
