using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Configuration;
using StreamStore.Storage;


namespace StreamStore.Sql.PostgreSql
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            services.ConfigurePersistence(new StorageConfigurator(PostgresConfiguration.DefaultConfiguration));
            return services;
        }

        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            services.ThrowIfNull(nameof(services));
            configuration.ThrowIfNull(nameof(configuration));
            return
                services.ConfigurePersistence(
                    new StorageConfigurator(
                        SqlStorageConfigurationBuilder.ReadFromConfig(
                            configuration,
                            PostgresConfiguration.ConfigurationSection,
                            PostgresConfiguration.DefaultConfiguration)));
        }

        public static IServiceCollection AddPostgres(this IServiceCollection services, Action<SqlStorageConfigurationBuilder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            services.ConfigurePersistence(
                new StorageConfigurator(
                    new SqlStorageConfigurationBuilder(
                        PostgresConfiguration.DefaultConfiguration, configure).Build()));
            return services;
        }

        public static IServiceCollection AddPostgresWithMultitenancy(this IServiceCollection services, SqlStorageConfiguration defaultConfig, Action<SqlMultitenancyConfigurator> configure)
        {
            configure.ThrowIfNull(nameof(defaultConfig));
            configure.ThrowIfNull(nameof(configure));
            services.ConfigurePersistence(
                new StorageConfigurator(defaultConfig),
                new MultitenancyConfigurator(configure));
            return services;
        }

        public static IServiceCollection AddPostgresWithMultitenancy(this IServiceCollection services, Action<SqlMultitenancyConfigurator> configure)
        {
            return services.AddPostgresWithMultitenancy(PostgresConfiguration.DefaultConfiguration, configure);
        }
    }
}
