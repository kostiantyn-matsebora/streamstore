using System;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Storage;

namespace StreamStore.NoSql.Cassandra
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCassandra(this IServiceCollection services, Action<ICassandraStorageDependencyBuilder> configure)
        {
            services.ThrowIfNull(nameof(services));
            configure.ThrowIfNull(nameof(configure));
            
            var storageConfigurator = new StorageConfigurator();
            configure(storageConfigurator);

            services.ConfigurePersistence(storageConfigurator);

            return services;
        }

        public static IServiceCollection AddCassandraWithMultitenancy(
            this IServiceCollection services, 
            Action<ICassandraStorageDependencyBuilder> configureStorage, 
            Action<ICassandraMultitenancyDependencyBuilder> configureMultitenancy)
        {
            services.ThrowIfNull(nameof(services));
            configureStorage.ThrowIfNull(nameof(configureStorage));
            configureMultitenancy.ThrowIfNull(nameof(configureMultitenancy));

            var storageConfigurator = new StorageConfigurator();
            configureStorage(storageConfigurator);
            
            var mode = storageConfigurator.GetCassandraMode();

            var multitenancyConfigurator = new MultitenancyConfigurator(mode);
            configureMultitenancy(multitenancyConfigurator);

            services.ConfigurePersistenceMultitenancy(storageConfigurator,multitenancyConfigurator);

            return services;
        }


        static (ICluster, IClusterConfigurator, CassandraStorageConfiguration) ConfigureStorage(Action<CassandraStorageDependencyBuilder> configure)
        {
            var builder = new CassandraStorageDependencyBuilder();
            configure(builder);
            return builder.Build();
        }
    }
}
