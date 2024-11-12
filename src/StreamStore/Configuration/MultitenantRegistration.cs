using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Multitenancy;
using System;
using System.Linq;

namespace StreamStore
{
    internal class MultitenantRegistration: DatabaseRegistrationBase
    {
        public override bool MultiTenancyEnabled => true;

        protected override void Validate(IServiceCollection services)
        {

            if (!services.Any(s => s.ServiceType == typeof(ITenantStreamDatabaseProvider)))
            {
                throw new InvalidOperationException("Database backend (ITenantStreamDatabaseProvider) is not registered");
            }
        }
    }
}
