using System;
using Cassandra.Mapping;


namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraMapper: IMapper, IDisposable
    {
    }
}
