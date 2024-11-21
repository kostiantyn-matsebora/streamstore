namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraStreamRepositoryFactory
    {
        ICassandraStreamRepository Create();
    }
}