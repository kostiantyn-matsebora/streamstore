using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Configuration
{
    public abstract class CassandraConfiguratorBase
    {
        Type sessionFactory = typeof(CassandraSessionFactory);

        CassandraStorageConfiguration storageConfig = new CassandraStorageConfiguration();

        public CassandraConfiguratorBase WithSessionFactory<TSessionFactory>() where TSessionFactory : ICassandraSessionFactory
        {
            sessionFactory = typeof(TSessionFactory);
            return this;
        }

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
            services.AddSingleton(typeof(ICassandraSessionFactory), sessionFactory);
            services.AddSingleton<ICassandraPredicateProvider, CassandraPredicateProvider>();
        }
    }
}
