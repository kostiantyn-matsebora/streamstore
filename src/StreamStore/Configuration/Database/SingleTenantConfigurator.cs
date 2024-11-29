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

        public ISingleTenantConfigurator UseDatabase<TDatabase>(Action<IServiceCollection>? dependencies = null) where TDatabase : IStreamDatabase
        {
            services.AddSingleton(typeof(IStreamDatabase), typeof(TDatabase));
            services.AddSingleton(typeof(IStreamReader), provider => provider.GetRequiredService<IStreamDatabase>());
            if (dependencies != null) dependencies.Invoke(services);
            ValidateConfiguration(services);
            return this;
        }

        public ISingleTenantConfigurator UseDatabase(IStreamDatabase database)
        {
            services.AddSingleton(typeof(IStreamDatabase), database);
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
            if (!services.Any(s => s.ServiceType == typeof(IStreamDatabase)))
            {
                throw new InvalidOperationException("Database backend (IStreamDatabase) is not registered");
            }

            if (!services.Any(s => s.ServiceType == typeof(IStreamReader)))
            {
                throw new InvalidOperationException("Database backend (IStreamReader) is not registered");
            }
        }
    }
}
