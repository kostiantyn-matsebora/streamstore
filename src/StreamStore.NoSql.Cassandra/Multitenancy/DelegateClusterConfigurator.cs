using System;
using System.Collections.Generic;
using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class DelegateClusterConfigurator : IClusterConfigurator
    {
        readonly List<Action<Builder>> configureActions = new List<Action<Builder>>();

        public DelegateClusterConfigurator AddConfigurator(Action<Builder> configure)
        {
            configureActions.Add(configure.ThrowIfNull(nameof(configure)));
            return this;
        }
        public void Configure(Builder builder)
        {
            foreach (var configure in configureActions)
                configure(builder);
        }
    }
}
