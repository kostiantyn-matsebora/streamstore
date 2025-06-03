using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Configuration;
using StreamStore.Storage;


namespace StreamStore.Sql.Sqlite
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSqliteStorage(this IServiceCollection services)
        {
            services.ConfigureStorage(new StorageConfigurator(SqliteConfiguration.DefaultConfiguration), new MultitenancyConfigurator());
            return services;
        }

        public static IServiceCollection AddSqliteStorage(this IServiceCollection services, IConfiguration configuration)
        {

            configuration.ThrowIfNull(nameof(configuration));
            var sqlConfig = SqlStorageConfigurationBuilder.ReadFromConfig(configuration, SqliteConfiguration.ConfigurationSection, SqliteConfiguration.DefaultConfiguration);
            services.ConfigureStorage(new StorageConfigurator(sqlConfig), new MultitenancyConfigurator());
            return services;
        }

        public static IServiceCollection AddSqliteStorage(this IServiceCollection services, Action<SqlStorageConfigurationBuilder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            var builder = new SqlStorageConfigurationBuilder(SqliteConfiguration.DefaultConfiguration);
            configure(builder);
            services.ConfigureStorage(new StorageConfigurator(builder.Build()), new MultitenancyConfigurator());
            return services;
        }
    }
}