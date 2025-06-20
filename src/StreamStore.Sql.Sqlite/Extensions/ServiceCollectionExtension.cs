﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Configuration;
using StreamStore.Storage;


namespace StreamStore.Sql.Sqlite
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseSqlite(this IServiceCollection services)
        {
            services.ConfigurePersistence(new StorageConfigurator(SqliteConfiguration.DefaultConfiguration));
            return services;
        }

        public static IServiceCollection UseSqlite(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.ThrowIfNull(nameof(configuration));
            services.ConfigurePersistence(
                new StorageConfigurator(
                        SqlStorageConfigurationBuilder.ReadFromConfig(
                            configuration,
                            SqliteConfiguration.ConfigurationSection,
                            SqliteConfiguration.DefaultConfiguration)));
            return services;
        }

        public static IServiceCollection UseSqlite(this IServiceCollection services, Action<SqlStorageConfigurationBuilder> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            services.ConfigurePersistence(
                new StorageConfigurator(
                    new SqlStorageConfigurationBuilder(
                        SqliteConfiguration.DefaultConfiguration, 
                        configure).Build()));
            return services;
        }

        public static IServiceCollection UseSqliteWithMultitenancy(this IServiceCollection services, SqlStorageConfiguration defaultConfig, Action<SqlMultitenancyConfigurator> configure)
        {
            configure.ThrowIfNull(nameof(defaultConfig));
            configure.ThrowIfNull(nameof(configure));
            services.ConfigurePersistenceMultitenancy(
                new StorageConfigurator(defaultConfig), 
                new MultitenancyConfigurator(configure));
            return services;
        }

        public static IServiceCollection UseSqliteWithMultitenancy(this IServiceCollection services, Action<SqlMultitenancyConfigurator> configure)
        {
            return services.UseSqliteWithMultitenancy(SqliteConfiguration.DefaultConfiguration, configure);
        }
    }
}
