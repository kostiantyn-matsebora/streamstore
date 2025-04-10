using System.Collections.Generic;
using StreamStore.Multitenancy;

namespace StreamStore.Provisioning
{
    class DefaultTenantProvider : ITenantProvider
    {
        readonly List<Id> tenants = new List<Id>();

        public DefaultTenantProvider()
        {
        }

        public DefaultTenantProvider(params Id[] tenants)
        {
            if (tenants != null) this.tenants.AddRange(tenants);
        }

        public IEnumerable<Id> GetAll()
        {
            return tenants;
        }
    }
}
