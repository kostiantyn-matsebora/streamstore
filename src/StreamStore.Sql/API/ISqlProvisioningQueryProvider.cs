using System;
using System.Collections.Generic;
using System.Text;

namespace StreamStore.Sql.API
{
    public interface ISqlProvisioningQueryProvider
    {
        public string GetSchemaProvisioningQuery();
    }
}
