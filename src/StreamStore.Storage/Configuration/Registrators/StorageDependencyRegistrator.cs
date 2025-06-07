using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;

namespace StreamStore.Storage.Configuration
{
    public sealed class StorageDependencyRegistrator
    {
        readonly IServiceCollection services = new ServiceCollection();
        public StorageDependencyRegistrator(IServiceCollection services)
        {
            this.services = services.ThrowIfNull(nameof(services));
        }

        public StorageDependencyRegistrator RegisterStorage<TStorage>() where TStorage : IStreamStorage
        {
            services.AddSingleton(typeof(IStreamStorage), typeof(TStorage));
            services.AddSingleton(typeof(IStreamReader), provider => provider.GetRequiredService<IStreamStorage>());
            services.AddSingleton(typeof(IStreamWriter), provider => provider.GetRequiredService<IStreamStorage>());

            return this;
        }
        public StorageDependencyRegistrator RegisterStorage(IStreamStorage storage)
        {
            services.AddSingleton(typeof(IStreamStorage), storage);
            services.AddSingleton(typeof(IStreamReader), storage);
            services.AddSingleton(typeof(IStreamWriter), storage);
            return this;
        }


        void ValidateConfiguration()
        {
            if (!services.Any(s => s.ServiceType == typeof(IStreamStorage)))
            {
                throw new InvalidOperationException("Storage backend (IStreamStorage) is not registered");
            }

            if (!services.Any(s => s.ServiceType == typeof(IStreamReader)))
            {
                throw new InvalidOperationException("Storage backend (IStreamReader) is not registered");
            }
        }
    }
}
