using System;
using System.Linq.Expressions;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraPredicateProvider
    {
        Expression<Func<EventEntity, bool>> StreamEvents(Id streamId);
        Expression<Func<EventEntity, bool>> StreamEvents(string streamId, int startFrom);
        Expression<Func<EventMetadataEntity, bool>> StreamMetadata(string streamId);
        Expression<Func<RevisionStreamEntity, bool>> StreamRevisions(string streamId);
    }
}