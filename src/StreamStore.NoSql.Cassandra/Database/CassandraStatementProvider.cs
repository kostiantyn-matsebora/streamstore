using Cassandra.Data.Linq;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStatementProvider
    {
        private readonly CassandraStatementConfigurator configure;
        private readonly ICassandraPredicateProvider predicates;

        public CassandraStatementProvider(CassandraStatementConfigurator configurator, ICassandraPredicateProvider predicates)
        {
            this.configure = configurator.ThrowIfNull(nameof(configurator));
            this.predicates = predicates.ThrowIfNull(nameof(predicates));
        }

        public CqlQuery<EventMetadataEntity> FindMetadata(Table<EventMetadataEntity> metadata, Id streamId)
        {
            return configure
                        .Query<CqlQuery<EventMetadataEntity>>(
                            metadata.Where(predicates.StreamMetadata(streamId)));
        }


        public CqlQuery<int> StreamRevisions(Table<RevisionStreamEntity> streamRevisions, Id streamId)
        {
            return configure
                        .Query<CqlQuery<int>>(
                            streamRevisions
                                .Where(predicates.StreamRevisions(streamId))
                                .Select(er => er.Revision));
        }

        public CqlQuery<EventEntity> StreamEvents(Table<EventEntity> events,Id streamId, Revision startFrom, int count)
        {
            return configure.Query<CqlQuery<EventEntity>>(
                    events
                        .Where(predicates.StreamEvents(streamId, startFrom))
                        .Take(count));
        }

        public CqlDelete DeleteStream(Table<EventEntity> events, Id streamId)
        {
            return events.Where(predicates.StreamEvents(streamId)).Delete();
        }
    }
}
