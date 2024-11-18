using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class DataContext
    {
        public readonly Table<EventEntity> Events;
        public readonly Table<EventMetadataEntity> Metadata;
        public readonly Table<RevisionStreamEntity> StreamRevisions;

        public DataContext(TypeMapFactory mapper, ISession session)
        {
            Events = new Table<EventEntity>(session, new MappingConfiguration().Define(mapper.CreateEventEntityMap()));
            Metadata = new Table<EventMetadataEntity>(session, new MappingConfiguration().Define(mapper.CreateEventMetadataMap()));
            StreamRevisions = new Table<RevisionStreamEntity>(session, new MappingConfiguration().Define(mapper.CreateStreamRevisionMap()));
        }
    }
}
