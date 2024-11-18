using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.API
{
    public interface ICassandraKeyspaceConfigurationProvider
    {
        CassandraKeyspaceConfiguration GetKeyspaceConfiguration(Id tenanId);

    }
}
