using System;
using System.Linq.Expressions;

using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraPredicateProvider : ICassandraPredicateProvider
    {

        public Expression<Func<RevisionStreamEntity, bool>> StreamRevisions(string streamId)
        {
            return er => er.StreamId == streamId;
        }

        public Expression<Func<EventEntity, bool>> StreamEvents(string streamId)
        {
            return er => er.StreamId == streamId;
        }

        public Expression<Func<EventMetadataEntity, bool>> StreamMetadata(string streamId)
        {
            return er => er.StreamId == streamId;
        }

        public Expression<Func<EventEntity, bool>> StreamEvents(string streamId, int startFrom)
        {
            return er => er.StreamId == streamId && er.Revision >= startFrom;
        }
    }
}
