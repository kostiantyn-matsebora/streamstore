using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using System;
using System.Linq;

namespace StreamStore.Configuration.Database
{
    public sealed class MultitenantDatabaseConfigurator : IMultitenantDatabaseConfigurator
    {
        readonly ServiceCollection services = new ServiceCollection();

        public MultitenantDatabaseConfigurator()
        {
            services.AddSingleton(typeof(ITenantSchemaProvisionerFactory), typeof(DefaultSchemaProvisionerFactory));
            services.AddSingleton(typeof(ITenantProvider), typeof(DefaultTenantProvider));
        }

        public IMultitenantDatabaseConfigurator UseDatabaseProvider<TDatabaseProvider>(Action<IServiceCollection>? dependencies = null) where TDatabaseProvider : ITenantStreamDatabaseProvider
        {
            services.AddSingleton(typeof(ITenantStreamDatabaseProvider), typeof(TDatabaseProvider));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }

        public IMultitenantDatabaseConfigurator UseSchemaProvisionerFactory<TFactory>(Action<IServiceCollection>? dependencies = null) where TFactory : ITenantSchemaProvisionerFactory
        {
            services.AddSingleton(typeof(ITenantSchemaProvisionerFactory), typeof(TFactory));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }

        public IMultitenantDatabaseConfigurator UseTenantProvider<TProvider>(Action<IServiceCollection>? dependencies = null) where TProvider : ITenantProvider
        {
            services.AddSingleton(typeof(ITenantProvider), typeof(TProvider));
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
            if (!services.Any(s => s.ServiceType == typeof(ITenantStreamDatabaseProvider)))
            {
                throw new InvalidOperationException("Database backend (ITenantStreamDatabaseProvider) is not registered");
            }
        }
    }
}
