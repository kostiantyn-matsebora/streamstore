using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;

namespace StreamStore.Configuration.Storage
{
    class SingleTenantConfigurator : ConfiguratorBase, ISingleTenantConfigurator
    {
        public SingleTenantConfigurator()
        {
            services.AddSingleton(typeof(ISchemaProvisioner), typeof(DefaultSchemaProvisioner));
        }

        public ISingleTenantConfigurator UseStorage<TStorage>(Action<IServiceCollection>? dependencies = null) where TStorage : IStreamStorage
        {
            services.AddSingleton(typeof(IStreamStorage), typeof(TStorage));
            services.AddSingleton(typeof(IStreamReader), provider => provider.GetRequiredService<IStreamStorage>());
            services.AddSingleton(typeof(IStreamWriter), provider => provider.GetRequiredService<IStreamStorage>());
            if (dependencies != null) dependencies.Invoke(services);
            ValidateConfiguration(services);
            return this;
        }

        public ISingleTenantConfigurator UseStorage(IStreamStorage storage)
        {
            services.AddSingleton(typeof(IStreamStorage), storage);
            services.AddSingleton(typeof(IStreamReader), storage);
            services.AddSingleton(typeof(IStreamWriter), storage);
            return this;
        }

        public ISingleTenantConfigurator UseSchemaProvisioner<TProvisioner>(Action<IServiceCollection>? dependencies = null) where TProvisioner : ISchemaProvisioner
        {
            services.AddSingleton(typeof(ISchemaProvisioner), typeof(TProvisioner));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }


        public IServiceCollection Configure()
        {
            ValidateConfiguration(services);
            return services;
        }

        static void ValidateConfiguration(IServiceCollection services)
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
