using System;
using System.Net.Security;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.Extensions;
using StreamStore.NoSql.Cassandra.Storage;


namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraStorageDependencyBuilder
    {
        readonly CassandraStorageConfiguration storageConfig = new CassandraStorageConfiguration();
        readonly Builder builder = new Builder();
        IServiceCollection services = new ServiceCollection();

        public CassandraStorageDependencyBuilder()
        {
            services.AddSingleton(builder);
            services.AddSingleton(storageConfig);
        }

        public CassandraStorageDependencyBuilder ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            var builder = new CassandraStorageConfigurationBuilder(storageConfig);
            configure(builder);
            builder.Build();
            return this;
        }

        public CassandraStorageDependencyBuilder ConfigureCluster(Action<Builder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            var builder = new Builder();
            configure(builder);
            services.AddSingleton<ICluster>(builder.Build());

            return this;
        }

        public CassandraStorageDependencyBuilder UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            connectionString.ThrowIfNullOrEmpty(nameof(connectionString));
            builder.WithCosmosDbConnectionString(connectionString!, remoteCertValidationCallback);
            storageConfig.Mode = CassandraMode.CosmosDbCassandra;
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
            return (services, storageConfig);
        }
    }
}
