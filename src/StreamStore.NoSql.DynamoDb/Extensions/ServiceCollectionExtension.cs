using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.DynamoDb.Configuration;
using StreamStore.Storage;

namespace StreamStore.NoSql.DynamoDb
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseDynamoDb(this IServiceCollection services)
        {
            return services.ConfigurePersistence(new StorageConfigurator());
        }

        public static IServiceCollection UseDynamoDb(this IServiceCollection services, Action<IDynamoDbConfigurator> configure)
        {
            return services;
        }
    }
}
