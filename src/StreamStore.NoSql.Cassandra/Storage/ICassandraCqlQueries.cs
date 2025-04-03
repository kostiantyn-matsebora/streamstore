using System;
using Cassandra.Mapping;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal interface ICassandraCqlQueries
    {
        Cql StreamActualRevision(string streamId);
        Cql DeleteStream(string streamId);
        Cql StreamEvents(string streamId, int from, int count);
        Cql CreateEventsTable();
    }
}
