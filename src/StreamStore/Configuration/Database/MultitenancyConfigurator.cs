using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using System;
using System.Linq;

namespace StreamStore.Configuration.Database
{
    public sealed class MultitenancyConfigurator : IMultitenancyConfigurator
    {
        readonly ServiceCollection services = new ServiceCollection();

        public IMultitenancyConfigurator UseDatabaseProvider<TDatabaseProvider>(Action<IServiceCollection>? dependencies = null) where TDatabaseProvider : ITenantStreamDatabaseProvider
        {
            services.AddSingleton(typeof(ITenantStreamDatabaseProvider), typeof(TDatabaseProvider));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }

        public IMultitenancyConfigurator UseSchemaProvisionerFactory<TFactory>(Action<IServiceCollection>? dependencies = null) where TFactory : ITenantSchemaProvisionerFactory
        {
            services.AddSingleton(typeof(ITenantSchemaProvisionerFactory), typeof(TFactory));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }

        public IMultitenancyConfigurator UseTenantProvider<TProvider>(Action<IServiceCollection>? dependencies = null) where TProvider : ITenantProvider
        {
            services.AddSingleton(typeof(ITenantProvider), typeof(TProvider));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }

        public IMultitenancyConfigurator WithTenants(params Id[] tenants)
        {
            services.AddSingleton(typeof(ITenantProvider), new DefaultTenantProvider(tenants));
            return this;
        }

        public IServiceCollection Configure()
        {
            ValidateConfiguration(services);

            if (!services.Any(s => s.ServiceType == typeof(ITenantProvider)))
            {
                services.AddSingleton<ITenantProvider, DefaultTenantProvider>();
            }
            if (!services.Any(s => s.ServiceType == typeof(ITenantSchemaProvisionerFactory)))
            {
                services.AddSingleton(typeof(ITenantSchemaProvisionerFactory), typeof(DefaultSchemaProvisionerFactory));
            }

            return services;
        }

        static void ValidateConfiguration(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(ITenantStreamDatabaseProvider)))
            {
                throw new InvalidOperationException("Database backend (ITenantStreamDatabaseProvider) is not registered");
            }
        }
    }
}
