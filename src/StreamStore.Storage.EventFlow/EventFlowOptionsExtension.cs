﻿using System;
using EventFlow;
using EventFlow.Extensions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;


namespace StreamStore.Storage.EventFlow
{
    public static class EventFlowOptionsExtension
    {
        public static IEventFlowOptions UseStreamStorageEventStore(this IEventFlowOptions eventFlowOptions, Action<IServiceCollection> configureStorage)
        {
            if (eventFlowOptions == null) throw new ArgumentNullException(nameof(eventFlowOptions));
            if (configureStorage == null) throw new ArgumentNullException(nameof(configureStorage));
            configureStorage(eventFlowOptions.ServiceCollection);
            return eventFlowOptions.UseEventPersistence<StreamStoragePersistence>();
        }

        public static IEventFlowOptions UseStreamStorageEventStore<TResolver>(this IEventFlowOptions eventFlowOptions, Action<IServiceCollection> configureStorage) where TResolver : class, ITenantIdResolver
        {
            if (eventFlowOptions == null) throw new ArgumentNullException(nameof(eventFlowOptions));
            if (configureStorage == null) throw new ArgumentNullException(nameof(configureStorage));
            
            configureStorage(eventFlowOptions.ServiceCollection);

            eventFlowOptions.ServiceCollection.AddScoped<ITenantIdResolver, TResolver>();
            eventFlowOptions.ServiceCollection.AddScoped(serviceProvider =>
            {
                var tenantIdResolver = serviceProvider.GetRequiredService<ITenantIdResolver>();
                var storageProvider = serviceProvider.GetRequiredService<ITenantStreamStorageProvider>();

                return storageProvider.GetStorage(tenantIdResolver.Resolve());
            });
            return eventFlowOptions.UseEventPersistence<StreamStoragePersistence>();
        }
    }
}
