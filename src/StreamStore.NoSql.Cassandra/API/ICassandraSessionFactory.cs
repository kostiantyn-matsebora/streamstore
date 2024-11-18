using Cassandra;

namespace StreamStore.NoSql.Cassandra.API
{
    public interface ICassandraSessionFactory
    {
        ISession CreateSession();
    }
}