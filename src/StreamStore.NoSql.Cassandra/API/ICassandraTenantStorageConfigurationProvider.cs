using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.API
{
    public interface ICassandraTenantStorageConfigurationProvider
    {
        CassandraStorageConfiguration GetConfiguration(Id tenanId);

    }
}
