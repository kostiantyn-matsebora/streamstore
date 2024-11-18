using System;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public abstract class CassandraConfiguratorBase
    {
        Type sessionFactory = typeof(CassandraSessionFactory);

        CassandraKeyspaceConfiguration keyspaceConfiguration = new CassandraKeyspaceConfiguration();

        public CassandraConfiguratorBase WithSessionFactory<TSessionFactory>() where TSessionFactory : ICassandraSessionFactory
        {
            sessionFactory = typeof(TSessionFactory);
            return this;
        }

        public CassandraConfiguratorBase WithKeyspaceConfiguration(Action<CassandraKeyspaceConfigurationBuilder> configure)
        {
            var builder = new CassandraKeyspaceConfigurationBuilder();
            configure(builder);
            keyspaceConfiguration = builder.Build();
            return this;
        }

        internal void Configure(IServiceCollection services)
        {
            ApplySharedDependencies(services);
            ApplySpecificDependencies(services);
        }

        protected abstract void ApplySpecificDependencies(IServiceCollection services);

        void ApplySharedDependencies(IServiceCollection services)
        {
            services.AddSingleton(keyspaceConfiguration);
            services.AddSingleton<TypeMapFactory>();
            services.AddSingleton<DataContextFactory>();
            services.AddSingleton(typeof(ICassandraSessionFactory), sessionFactory);
        }

    }
}
