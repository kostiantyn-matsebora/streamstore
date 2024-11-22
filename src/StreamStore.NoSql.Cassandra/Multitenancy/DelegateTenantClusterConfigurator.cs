using System;
using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{

    internal class DelegateTenantClusterConfigurator: ITenantClusterConfigurator
    {
        readonly Action<Id, Builder> configure;

        public DelegateTenantClusterConfigurator() : this((tenantId, builder) => { })
        {
        }

        public DelegateTenantClusterConfigurator(Action<Id, Builder> configure)
        {
            this.configure = configure.ThrowIfNull(nameof(configure));
        }

        public void Configure(Id tenantId, Builder builder)
        {
            configure(tenantId, builder);
        }
    }
}
