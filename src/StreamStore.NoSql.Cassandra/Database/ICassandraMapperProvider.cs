
using Cassandra.Mapping;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraMapperProvider
    {
        public IMapper OpenMapper();
    }
}
