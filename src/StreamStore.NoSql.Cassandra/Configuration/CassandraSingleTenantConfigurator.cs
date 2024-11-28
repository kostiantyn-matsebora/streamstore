using System;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Extensions;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CassandraSingleTenantConfigurator: CassandraConfiguratorBase
    {
        protected Builder builder = Cluster.Builder();
        Type sessionFactory = typeof(CassandraSessionFactory);



        public CassandraConfiguratorBase WithSessionFactory<TSessionFactory>() where TSessionFactory : ICassandraSessionFactory
        {
            sessionFactory = typeof(TSessionFactory);
            return this;
        }

        public CassandraSingleTenantConfigurator ConfigureCluster(Action<Builder> configure)
        {
           configure(builder);
           return this;
        }

        public CassandraSingleTenantConfigurator WithCredentials(string username, string password)
        {
            builder.WithCredentials(username, password);
            return this;
        }

        public CassandraSingleTenantConfigurator ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
           ConfigureStorageInstance(configure);
           return this;
        }

        public CassandraSingleTenantConfigurator UseCosmosDb()
        {
            mode = CassandraMode.CosmosDbCassandra;
            return this;
        }

        public CassandraSingleTenantConfigurator UseAppConfig(IConfiguration configuration, string connectionStringName = "StreamStore")
        {
            builder.UseAppConfig(configuration, connectionStringName);
            return this;
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
}
