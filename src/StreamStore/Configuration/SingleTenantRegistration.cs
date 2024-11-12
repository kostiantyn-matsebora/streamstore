using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using System;
using System.Linq;

namespace StreamStore
{
    internal class SingleTenantRegistration : DatabaseRegistrationBase
    {
        public override bool MultiTenancyEnabled => false;

        protected override void Validate(IServiceCollection services)
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
