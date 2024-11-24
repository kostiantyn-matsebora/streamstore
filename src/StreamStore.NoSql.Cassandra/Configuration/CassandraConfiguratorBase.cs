using System;
using Cassandra.Mapping;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Configuration
{
    public abstract class CassandraConfiguratorBase
    {


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

        protected abstract void ApplySpecificDependencies(IServiceCollection services);

        void ApplySharedDependencies(IServiceCollection services)
        {
            services.AddSingleton(storageConfig);
        }
    }
}
