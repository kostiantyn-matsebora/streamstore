using System;
using System.Net.Security;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Extensions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    internal class StorageConfigurator : StorageConfiguratorBase, ICassandraStorageDependencyBuilder
    {
        readonly CassandraStorageConfigurationBuilder configBuilder = new CassandraStorageConfigurationBuilder();
        readonly DelegateClusterConfigurator clusterConfigurator = new DelegateClusterConfigurator();

        public StorageConfigurator()
        {
        }

        public ICassandraStorageDependencyBuilder ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            configure(configBuilder);
            return this;
        }

        public ICassandraStorageDependencyBuilder ConfigureCluster(Action<Builder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            clusterConfigurator.AddConfigurator(configure);
            return this;
        }

        public ICassandraStorageDependencyBuilder UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            connectionString.ThrowIfNullOrEmpty(nameof(connectionString));
            clusterConfigurator.AddConfigurator(builder => builder.WithCosmosDbConnectionString(connectionString!, remoteCertValidationCallback));
            configBuilder.WithMode(CassandraMode.CosmosDbCassandra);
            return this;
        }

        public ICassandraStorageDependencyBuilder UseCosmosDb(IConfiguration configuration, string connectionStringName = "StreamStore", RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string {connectionStringName} is not found in configuration", nameof(configuration));
            }

            return UseCosmosDb(connectionString, remoteCertValidationCallback);
        }

        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
            registrator.RegisterStorage<CassandraStreamStorage>();
        }

        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioner<CassandraSchemaProvisioner>();
        }

        internal CassandraMode GetCassandraMode()
        {
            return configBuilder.Build().Mode;
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            var builder = new Builder();
            clusterConfigurator.Configure(builder);
            var storageConfig = configBuilder.Build();

            services
                .AddSingleton<IClusterConfigurator>(clusterConfigurator)
                .AddSingleton<ICluster>((serviceProvider) => builder.Build())
                .AddSingleton(storageConfig)
                .AddSingleton<ICassandraSessionFactory, CassandraSessionFactory>()
                .AddSingleton(typeof(ICassandraMapperProvider), typeof(CassandraMapperProvider))
                .AddSingleton(new MappingConfiguration().Define(new CassandraStreamMapping(storageConfig)))
                .AddSingleton<ICassandraCqlQueries>(new CassandraCqlQueriesProvider(storageConfig.Mode).GetCqlQueries(storageConfig));
        }
    }
}
