﻿using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Provisioning;
using StreamStore.Storage.Provisioning;

namespace StreamStore.Storage.Configuration
{
    public sealed class SchemaProvisionerRegistrator
    {
        readonly IServiceCollection services;

        public SchemaProvisionerRegistrator(IServiceCollection services)
        {
            this.services = services.ThrowIfNull(nameof(services));
        }
        public IServiceCollection RegisterSchemaProvisioner<TProvisioner>() where TProvisioner : ISchemaProvisioner
        {
            services.AddSingleton(typeof(ISchemaProvisioner), typeof(TProvisioner));
            return services;
        }

        public IServiceCollection RegisterDummySchemaProvisioner()
        {
            return RegisterSchemaProvisioner<NoopSchemaProvisioner>();
        }
    }
}
