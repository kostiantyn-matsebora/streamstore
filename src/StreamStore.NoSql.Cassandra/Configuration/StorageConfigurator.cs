using Cassandra.Mapping;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        readonly IServiceCollection services;
        readonly CassandraStorageConfiguration config;

        public StorageConfigurator(IServiceCollection services, CassandraStorageConfiguration config)
        {
          this.services = services.ThrowIfNull(nameof(services));
          this.config = config.ThrowIfNull(nameof(config));
        }

        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
            registrator.RegisterStorage<CassandraStreamStorage>();
        }

        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioner<CassandraSchemaProvisioner>();
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            var serviceProvider = this.services.BuildServiceProvider();
            services
                .CopyFrom(this.services)
                .AddSingleton(config)
                .AddSingleton<ICassandraSessionFactory, CassandraSessionFactory>()
                .AddSingleton(typeof(ICassandraMapperProvider), typeof(CassandraMapperProvider))
                .AddSingleton(new MappingConfiguration().Define(new CassandraStreamMapping(config)))
                .AddSingleton<ICassandraCqlQueries>(new CassandraCqlQueriesProvider(config.Mode).GetCqlQueries(config));
        }


    }
}
