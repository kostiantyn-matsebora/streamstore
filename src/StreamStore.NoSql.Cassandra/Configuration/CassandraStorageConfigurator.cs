//using System;
//using System.Net.Security;
//using Cassandra;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.Extensions;
//using StreamStore.NoSql.Cassandra.Extensions;

//namespace StreamStore.NoSql.Cassandra.Configuration
//{
//    internal class CassandraStorageDependencyBuilder
//    {
//        Builder builder = Cluster.Builder();
//        readonly IServiceCollection services;

//        public CassandraStorageDependencyBuilder(IServiceCollection services)
//        {
//            this.services = services.ThrowIfNull(nameof(services));
//        }

//        public CassandraStorageDependencyBuilder ConfigureCluster(Action<Builder> configure)
//        {
//            configure(builder);
//            services.AddSingleton<ICluster>(builder.Build());
//            return this;
//        }

//        public CassandraStorageDependencyBuilder ConfigurePersistence(Action<CassandraStorageConfigurationBuilder> configure)
//        {
//            var builder = new CassandraStorageConfigurationBuilder();
//            configure(builder);
//            services.AddSingleton(builder.Build());
//            return this;
//        }

//        public CassandraStorageDependencyBuilder UseCosmosDb(IConfiguration configuration, string connectionStringName = "StreamStore", RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
//        {
//            var connectionString = configuration.GetConnectionString(connectionStringName);

//            if (string.IsNullOrEmpty(connectionString))
//            {
//                throw new ArgumentException($"Connection string {connectionStringName} is not found in configuration", nameof(configuration));
//            }

//            return UseCosmosDb(connectionString, remoteCertValidationCallback);
//        }

//        public CassandraStorageDependencyBuilder UseCosmosDb(string? connectionString = null, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
//        {
//            connectionString.ThrowIfNullOrEmpty(nameof(connectionString));
//            builder.WithCosmosDbConnectionString(connectionString!, remoteCertValidationCallback);
//            services.AddSingleton<ICluster>(builder.Build());
//            return this;
//        }
//    }
//}
