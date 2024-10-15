﻿using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStreamStore(this IServiceCollection services)
        {
            services.AddSingleton<IStreamStore, StreamStore>();
            services.AddSingleton<IEventSerializer, EventSerializer>();
            return services;
        }
    }
}