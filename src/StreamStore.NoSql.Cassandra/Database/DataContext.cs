using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class DataContext
    {
        public readonly Table<EventEntity> Events;
        public readonly Table<EventEntity> Metadata;
        public readonly Table<EventPerStreamEntity> EventPerStream;
        public readonly Table<RevisionPerStreamEntity> RevisionPerStream;

        public DataContext(TypeMapFactory mapper, ISession session)
        {
            Events = new Table<EventEntity>(session, new MappingConfiguration().Define(mapper.CreateEventEntityMap()));
            Metadata = new Table<EventEntity>(session, new MappingConfiguration().Define(mapper.CreateEventMetadataMap()));
            EventPerStream = new Table<EventPerStreamEntity>(session, new MappingConfiguration().Define(mapper.CreateEventPerStreamMap()));
            RevisionPerStream = new Table<RevisionPerStreamEntity>(session, new MappingConfiguration().Define(mapper.CreateRevisionPerStreamMap()));
        }
    }
}
