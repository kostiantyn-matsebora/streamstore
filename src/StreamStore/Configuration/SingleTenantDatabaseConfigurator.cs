using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using System;
using System.Linq;

namespace StreamStore
{
    public class SingleTenantDatabaseConfigurator : ISingleTenantDatabaseConfigurator
    {
        readonly ServiceCollection services = new ServiceCollection();

        public SingleTenantDatabaseConfigurator()
        {
            services.AddSingleton(typeof(ISchemaProvisioner), typeof(DefaultSchemaProvisioner));
        }

        public ISingleTenantDatabaseConfigurator UseDatabase<TDatabase>(Action<IServiceCollection>? dependencies = null) where TDatabase : IStreamDatabase
        {
            services.AddSingleton(typeof(IStreamDatabase), typeof(TDatabase));
            services.AddSingleton(typeof(IStreamReader), typeof(TDatabase));
            if (dependencies != null) dependencies.Invoke(services);
            ValidateConfiguration(services);
            return this;
        }

        public ISingleTenantDatabaseConfigurator UseDatabase(IStreamDatabase database)
        {
            services.AddSingleton(typeof(IStreamDatabase), database);
            services.AddSingleton(typeof(IStreamReader), database);
            return this;
        }

        public ISingleTenantDatabaseConfigurator UseSchemaProvisioner<TProvisioner>(Action<IServiceCollection>? dependencies = null) where TProvisioner : ISchemaProvisioner
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

        void ValidateConfiguration(IServiceCollection services)
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
