using System.Collections.Generic;
using StreamStore.Multitenancy;

namespace StreamStore.ExampleBase.Multitenancy
{
    internal class TenantQueue
    {
        Queue<Id> tenants;
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
