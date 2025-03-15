﻿using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;


namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraMapperProvider : ICassandraMapperProvider
    {
        readonly ICassandraSessionFactory sessionFactory;
        readonly MappingConfiguration mapping;

        public CassandraMapperProvider(ICassandraSessionFactory sessionFactory, MappingConfiguration mapping)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.mapping = mapping.ThrowIfNull(nameof(mapping));
        }

        public IMapper OpenMapper()
        {
            return new Mapper(sessionFactory.Open(), mapping);
        }
    }
}
