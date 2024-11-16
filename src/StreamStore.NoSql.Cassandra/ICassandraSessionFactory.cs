using Cassandra;

namespace StreamStore.NoSql.Cassandra
{
    internal interface ICassandraSessionFactory
    {
        ISession CreateSession();
    }
}