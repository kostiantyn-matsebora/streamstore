using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Configuration;
using StreamStore.Storage;


namespace StreamStore.Sql.PostgreSql.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services)
        {
            services.ConfigureStorage(new StorageConfigurator(PostgresConfiguration.DefaultConfiguration), new MultitenancyConfigurator());
            return services;
        }

        public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.ThrowIfNull(nameof(services));
            configuration.ThrowIfNull(nameof(configuration));
            return 
                services.ConfigureStorage(
                    new StorageConfigurator(SqlStorageConfigurationBuilder.ReadFromConfig(
                        configuration, PostgresConfiguration.ConfigurationSection, PostgresConfiguration.DefaultConfiguration)), 
                    new MultitenancyConfigurator());
        }

        public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection services, Action<SqlStorageConfigurationBuilder> configure)
        {
            services.ThrowIfNull(nameof(services));
            configure.ThrowIfNull(nameof(configure));

            var builder = new SqlStorageConfigurationBuilder(PostgresConfiguration.DefaultConfiguration);
            configure(builder);

            return services.ConfigureStorage(new StorageConfigurator(builder.Build()), new MultitenancyConfigurator());
        }
    }
}
