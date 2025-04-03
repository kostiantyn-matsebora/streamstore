using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using System;
using System.Linq;

namespace StreamStore.Configuration.Database
{
    public class SingleTenantConfigurator : ISingleTenantConfigurator
    {
        readonly ServiceCollection services = new ServiceCollection();

        public SingleTenantConfigurator()
        {
            services.AddSingleton(typeof(ISchemaProvisioner), typeof(DefaultSchemaProvisioner));
        }

        public ISingleTenantConfigurator UseDatabase<TDatabase>(Action<IServiceCollection>? dependencies = null) where TDatabase : IStreamStorage
        {
            services.AddSingleton(typeof(IStreamStorage), typeof(TDatabase));
            services.AddSingleton(typeof(IStreamReader), provider => provider.GetRequiredService<IStreamStorage>());
            if (dependencies != null) dependencies.Invoke(services);
            ValidateConfiguration(services);
            return this;
        }

        public ISingleTenantConfigurator UseDatabase(IStreamStorage database)
        {
            services.AddSingleton(typeof(IStreamStorage), database);
            services.AddSingleton(typeof(IStreamReader), database);
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
                throw new InvalidOperationException("Database backend (IStreamStorage) is not registered");
            }

            if (!services.Any(s => s.ServiceType == typeof(IStreamReader)))
            {
                throw new InvalidOperationException("Database backend (IStreamReader) is not registered");
            }
        }
    }
}
