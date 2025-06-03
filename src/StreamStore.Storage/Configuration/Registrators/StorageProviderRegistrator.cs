using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Multitenancy;
using StreamStore.Storage.Multitenancy;


namespace StreamStore.Storage.Configuration
{
    public sealed class StorageProviderRegistrator
    {
        readonly IServiceCollection services;

        public StorageProviderRegistrator(IServiceCollection services)
        {
          this.services = services.ThrowIfNull(nameof(services));
        }

        public IServiceCollection RegisterStorageProvider(Func<IServiceProvider, Func<Id,IStreamStorage>> provider)
        {
            services.AddSingleton<ITenantStreamStorageProvider>(sp => new DelegateStorageProvider(provider(sp)));
            return services;
        }
    }
}
