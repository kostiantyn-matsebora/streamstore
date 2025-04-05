using System;
using System.Net.Security;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Storage;
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

        public CassandraSingleTenantConfigurator ConfigureStorage(Action<CassandraStorageConfigurationBuilder> configure)
        {
           ConfigureStorageInstance(configure);
           return this;
        }

        public CassandraSingleTenantConfigurator UseCosmosDb(IConfiguration configuration, string connectionStringName = "StreamStore", RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string {connectionStringName} is not found in configuration", nameof(configuration));
            }

            return UseCosmosDb(connectionString, remoteCertValidationCallback);
        }

        public CassandraSingleTenantConfigurator UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {
            connectionString.ThrowIfNullOrEmpty(nameof(connectionString));
            builder.WithCosmosDbConnectionString(connectionString!, remoteCertValidationCallback);
            mode = CassandraMode.CosmosDbCassandra;
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
