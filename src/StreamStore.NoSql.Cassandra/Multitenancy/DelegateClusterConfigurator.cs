using System;
using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class DelegateClusterConfigurator : IClusterConfigurator
    {
        readonly Action<Builder> configure;

        public DelegateClusterConfigurator(Action<Builder> configure)
        {
            this.configure = configure.ThrowIfNull(nameof(configure));
        }

        public void Configure(Builder builder)
        {
            configure(builder);
        }
    }
}
