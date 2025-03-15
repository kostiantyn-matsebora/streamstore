using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    [ExcludeFromCodeCoverage]
    internal class TenantQueue
    {
        readonly Queue<Id> tenants;
        public TenantQueue(ITenantProvider tenantProvider)
        {
            tenants = new Queue<Id>(tenantProvider.GetAll());
        }
        public Id DequeueTenant()
        {
            return tenants.Dequeue();
        }
    }
}
