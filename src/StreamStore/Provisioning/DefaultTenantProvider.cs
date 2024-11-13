using System.Collections.Generic;
using System.Linq;
using StreamStore.Multitenancy;

namespace StreamStore.Provisioning
{
    internal class DefaultTenantProvider : ITenantProvider
    {
        public IEnumerable<Id> GetAll()
        {
            return Enumerable.Empty<Id>();
        }
    }
}
