using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    interface IClusterConfigurator
    {
        void Configure(Builder builder);
    }
}
