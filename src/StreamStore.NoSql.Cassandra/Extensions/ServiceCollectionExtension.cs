using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.Storage;

namespace StreamStore.NoSql.Cassandra
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCassandra(this IServiceCollection services, Action<CassandraStorageDependencyBuilder> configure)
        {
            services.ThrowIfNull(nameof(services));
            configure.ThrowIfNull(nameof(configure));
            
            (var storageServices, var config) = ConfigureStorage(configure);

            services.ConfigurePersistence(new StorageConfigurator(storageServices, config));

            return services;
        }

        public static IServiceCollection AddCassandraWithMultitenancy(
            this IServiceCollection services, 
            Action<CassandraStorageDependencyBuilder> configureStorage, 
            Action<CassandraMultitenancyDependencyBuilder> configureMultitenancy)
        {
            services.ThrowIfNull(nameof(services));
            configureStorage.ThrowIfNull(nameof(configureStorage));
            configureMultitenancy.ThrowIfNull(nameof(configureMultitenancy));

            (var storageServices, var config) = ConfigureStorage(configureStorage);

            services.ConfigurePersistenceMultitenancy(
                new StorageConfigurator(storageServices, config),
                new MultitenancyConfigurator(ConfigureMultitenancy(configureMultitenancy), config.Mode));

            return services;
        }

        static IServiceCollection ConfigureMultitenancy(Action<CassandraMultitenancyDependencyBuilder> configureMultitenancy)
        {
            var multitenancyBuilder = new CassandraMultitenancyDependencyBuilder();
            configureMultitenancy(multitenancyBuilder);
            return multitenancyBuilder.Build();
        }

        static (IServiceCollection, CassandraStorageConfiguration) ConfigureStorage(Action<CassandraStorageDependencyBuilder> configure)
        {
            var builder = new CassandraStorageDependencyBuilder();
            configure(builder);
            return builder.Build();
        }
    }
}
