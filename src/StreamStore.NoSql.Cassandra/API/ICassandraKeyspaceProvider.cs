namespace StreamStore.NoSql.Cassandra.API
{
    public interface ICassandraKeyspaceProvider
    {
        public string GetKeyspace(Id tenantId);
    }
}
