
using Cassandra.Mapping;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal interface ICassandraMapperProvider
    {
        public IMapper OpenMapper();
    }
}
