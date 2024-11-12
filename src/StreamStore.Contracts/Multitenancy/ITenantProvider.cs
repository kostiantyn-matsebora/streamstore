using System.Collections;
using System.Collections.Generic;

namespace StreamStore.Multitenancy
{
    public interface ITenantProvider
    {
        IEnumerable<Id> GetAll();
    }
}
