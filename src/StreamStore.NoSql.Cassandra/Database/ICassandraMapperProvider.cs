
namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraMapperProvider
    {
        public ICassandraMapper OpenMapper();
    }
}
