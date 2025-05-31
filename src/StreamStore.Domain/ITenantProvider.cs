using System.Collections;
using System.Collections.Generic;

namespace StreamStore
{
    public interface ITenantProvider
    {
        IEnumerable<Id> GetAll();
    }
}
