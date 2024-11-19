using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.API
{
    public interface ICassandraStorageConfigurationProvider
    {
        CassandraStorageConfiguration GetStorageConfiguration(Id tenanId);

    }
}
