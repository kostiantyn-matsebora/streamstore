using System;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraSingleTenantConfigurator: CassandraConfiguratorBase
    {
        Cluster? cluster;
        Type sessionFactory = typeof(CassandraSessionFactory);
        public CassandraConfiguratorBase WithSessionFactory<TSessionFactory>() where TSessionFactory : ICassandraSessionFactory
        {
            sessionFactory = typeof(TSessionFactory);
            return this;
        }

        public CassandraSingleTenantConfigurator ConfigureCluster(Action<Builder> configure)
        {
           var builder = Cluster.Builder();
           configure(builder);
           cluster = builder.Build();
           return this;
        }

        public CassandraConfiguratorBase ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
           ConfigureStorageInstance(configure);
           return this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
          if (cluster == null) throw new InvalidOperationException("Cluster not configured");
          services.AddSingleton(cluster);
          services.AddSingleton<ICassandraStreamRepositoryFactory, CassandraStreamRepositoryFactory>();
          services.AddSingleton(typeof(ICassandraSessionFactory), sessionFactory);
          services.AddSingleton(typeof(ICassandraMapperProvider), typeof(CassandraMapperProvider));
          services.AddSingleton(new MappingConfiguration().Define(new CassandraStreamMapping(storageConfig)));
        }
    }
}
