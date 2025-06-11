using System;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    internal class MultitenancyConfigurator : MultitenancyConfiguratorBase, ICassandraMultitenancyDependencyBuilder
    {
        readonly IServiceCollection services = new ServiceCollection();
        readonly CassandraMode mode;
        DelegateTenantClusterConfigurator tenantClusterConfigurator = new DelegateTenantClusterConfigurator();
        readonly CassandraKeyspaceRegistry keyspaceProvider = new CassandraKeyspaceRegistry();

        public MultitenancyConfigurator(CassandraMode mode)
        {
            this.mode = mode.ThrowIfNull(nameof(mode));

            services.AddSingleton<ITenantClusterConfigurator>(tenantClusterConfigurator);
            services.AddSingleton<ICassandraTenantStorageConfigurationProvider, CassandraStorageConfigurationProvider>();
            services.AddSingleton<ICassandraKeyspaceProvider>(keyspaceProvider);
        }

        public ICassandraMultitenancyDependencyBuilder WithTenantClusterConfigurator(Action<Id, Builder> configurator)
        {
            tenantClusterConfigurator = new DelegateTenantClusterConfigurator(configurator);
            services.AddSingleton<ITenantClusterConfigurator>(tenantClusterConfigurator);
            return this;
        }

        public ICassandraMultitenancyDependencyBuilder WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
        {
            services.AddSingleton(typeof(ICassandraTenantStorageConfigurationProvider), typeof(TStorageConfigurationProvider));
            return this;
        }


        public ICassandraMultitenancyDependencyBuilder WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider
        {
            services.AddSingleton(typeof(ICassandraKeyspaceProvider), typeof(TKeyspaceProvider));
            return this;
        }

        public ICassandraMultitenancyDependencyBuilder AddKeyspace(Id tenantId, string keyspace)
        {
            keyspaceProvider.AddKeyspace(tenantId, keyspace);
            return this;
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioningFactory(serviceProvider => serviceProvider.GetRequiredService<CassandraSchemaProvisionerFactory>().Create);
        }

        protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
        {
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<CassandraStreamStorageProvider>().GetStorage);
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            services.CopyFrom(this.services);

            services.AddSingleton(typeof(ICassandraTenantMapperProvider), typeof(CassandraTenantMapperProvider));
            services.AddSingleton<ICassandraTenantClusterRegistry, CassandraTenantClusterRegistry>();
            services.AddSingleton<ICassandraTenantMappingRegistry, CassandraTenantMappingRegistry>();
            services.AddSingleton<ICassandraCqlQueriesProvider>(new CassandraCqlQueriesProvider(mode));
            services.AddSingleton<CassandraSchemaProvisionerFactory>();
            services.AddSingleton<CassandraStreamStorageProvider>();
        }
    }
}
