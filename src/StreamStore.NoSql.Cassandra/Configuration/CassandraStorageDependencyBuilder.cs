using System;
using System.Net.Security;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.Extensions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Storage;


namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraStorageDependencyBuilder
    {
        readonly CassandraStorageConfigurationBuilder configBuilder = new CassandraStorageConfigurationBuilder();
        readonly DelegateClusterConfigurator clusterConfigurator = new DelegateClusterConfigurator();

        public CassandraStorageDependencyBuilder()
        {
        }

        public CassandraStorageDependencyBuilder ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            configure(configBuilder);
            return this;
        }

        public CassandraStorageDependencyBuilder ConfigureCluster(Action<Builder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            clusterConfigurator.AddConfigurator(configure);
            return this;
        }

        public CassandraStorageDependencyBuilder UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            connectionString.ThrowIfNullOrEmpty(nameof(connectionString));
            clusterConfigurator.AddConfigurator(builder => builder.WithCosmosDbConnectionString(connectionString!, remoteCertValidationCallback));
            configBuilder.WithMode(CassandraMode.CosmosDbCassandra);
            return this;
        }

        public CassandraStorageDependencyBuilder UseCosmosDb(IConfiguration configuration, string connectionStringName = "StreamStore", RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string {connectionStringName} is not found in configuration", nameof(configuration));
            }

            return UseCosmosDb(connectionString, remoteCertValidationCallback);
        }

        public (IServiceCollection, CassandraStorageConfiguration) Build()
        {
            var builder = new Builder();
            clusterConfigurator.Configure(builder);
            var storageConfig = configBuilder.Build();

            var services =
                new ServiceCollection()
                .AddSingleton<IClusterConfigurator>(clusterConfigurator)
                .AddSingleton<ICluster>(builder.Build())
                .AddSingleton(storageConfig);


            return (services, storageConfig);
        }
    }
}
