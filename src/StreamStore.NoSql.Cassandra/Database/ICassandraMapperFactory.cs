using System;
using System.Collections.Generic;
using System.Text;
using Cassandra;
using Cassandra.Mapping;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraMapperFactory
    {
        public IMapper CreateMapper(ISession session)
        {
            return new Mapper(session);
        }
    }

    internal class CassandraMapperFactory : ICassandraMapperFactory
    {
        readonly MappingConfiguration mapping;

        public CassandraMapperFactory(MappingConfiguration mapping)
        {
            this.mapping = mapping.ThrowIfNull(nameof(mapping));
        }

        public IMapper CreateMapper(ISession session)
        {
            return new Mapper(session, mapping);
        }
    }
}
