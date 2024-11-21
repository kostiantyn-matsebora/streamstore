using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraStreamRepository: IDisposable
    {
        Task<AppliedInfo<EventEntity>> AppendToStream(Id streamId, params EventRecord[] records);
        Task CreateSchemaIfNotExistsAsync();
        Task DeleteStream(Id streamId);
        Task<IEnumerable<EventMetadataEntity>> FindMetadata(Id streamId);
        Task<IEnumerable<EventEntity>> GetEvents(Id streamId, Revision startFrom, int count);
        Task<int> GetStreamActualRevision(Id streamId);
    }
}