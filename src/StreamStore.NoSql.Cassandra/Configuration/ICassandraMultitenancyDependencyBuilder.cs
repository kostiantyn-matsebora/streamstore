using StreamStore.NoSql.Cassandra.API;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public interface ICassandraMultitenancyDependencyBuilder
    {
        ICassandraMultitenancyDependencyBuilder WithStorageConfigurationProvider<TStorageConfigurationProvider>() where TStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider;
        ICassandraMultitenancyDependencyBuilder WithKeyspaceProvider<TKeyspaceProvider>() where TKeyspaceProvider : ICassandraKeyspaceProvider;
        ICassandraMultitenancyDependencyBuilder AddKeyspace(Id tenantId, string keyspace);
    }
}
