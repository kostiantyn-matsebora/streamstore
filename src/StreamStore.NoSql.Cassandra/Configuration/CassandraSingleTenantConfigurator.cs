using System;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CassandraSingleTenantConfigurator<TConfigurator>: CassandraConfiguratorBase where TConfigurator: CassandraSingleTenantConfigurator<TConfigurator>
    {
        protected Builder builder = Cluster.Builder();
        Type sessionFactory = typeof(CassandraSessionFactory);


        public CassandraConfiguratorBase WithSessionFactory<TSessionFactory>() where TSessionFactory : ICassandraSessionFactory
        {
            sessionFactory = typeof(TSessionFactory);
            return this;
        }

        public TConfigurator ConfigureCluster(Action<Builder> configure)
        {
           configure(builder);
           return (TConfigurator)this;
        }

        public TConfigurator WithCredentials(string username, string password)
        {
            builder.WithCredentials(username, password);
            return (TConfigurator)this;
        }

        public TConfigurator ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
           ConfigureStorageInstance(configure);
           return (TConfigurator)this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
          services.AddSingleton(builder.Build());
          services.AddSingleton(typeof(ICassandraSessionFactory), sessionFactory);
          services.AddSingleton(typeof(ICassandraMapperProvider), typeof(CassandraMapperProvider));
          services.AddSingleton(new MappingConfiguration().Define(new CassandraStreamMapping(storageConfig)));
          services.AddSingleton<ICassandraCqlQueries>(new CassandraCqlQueriesProvider(mode).GetCqlQueries(storageConfig));
        }
    }

    public class CassandraSingleTenantConfigurator : CassandraSingleTenantConfigurator<CassandraSingleTenantConfigurator>
    {
    }
}
