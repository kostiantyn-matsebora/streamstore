using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Multitenancy;
using System;
using System.Linq;

namespace StreamStore
{
    public sealed class MultitenantDatabaseRegistrator: DatabaseRegistratorBase, IMultitenantDatabaseRegistrator
    {
        protected override void ApplyInternal(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        protected override void Validate(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(ITenantStreamDatabaseProvider)))
            {
                throw new InvalidOperationException("Database backend (ITenantStreamDatabaseProvider) is not registered");
            }
        }
    }
}
