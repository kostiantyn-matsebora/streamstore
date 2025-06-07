using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    internal class MultitenancyConfigurator : MultitenancyConfiguratorBase
    {
        readonly IServiceCollection services;
        readonly CassandraMode mode;

        public MultitenancyConfigurator(IServiceCollection services, CassandraMode mode)
        {
            this.services = services.ThrowIfNull(nameof(services));
            this.mode = mode.ThrowIfNull(nameof(mode));

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
