using System;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.Database;



namespace StreamStore.NoSql.Cassandra.Configuration
{
    public abstract class CassandraConfiguratorBase
    {

        internal CassandraMode mode = CassandraMode.Cassandra;

        protected CassandraStorageConfiguration storageConfig = new CassandraStorageConfiguration();

        protected void ConfigureStorageInstance(Action<CassandraStorageConfigurationBuilder> configure)
        {
            var builder = new CassandraStorageConfigurationBuilder();
            configure(builder);
            storageConfig = builder.Build();
        }

        internal void Configure(IServiceCollection services)
        {
            ApplySharedDependencies(services);
            ApplySpecificDependencies(services);
        }

        internal void UseAppConfig(IConfiguration configuration, string connectionStringName,  Builder builder)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");
            }

            builder.WithConnectionString(connectionString);
        }

        protected abstract void ApplySpecificDependencies(IServiceCollection services);

        void ApplySharedDependencies(IServiceCollection services)
        {
            services.AddSingleton(storageConfig);
        }
    }
}
