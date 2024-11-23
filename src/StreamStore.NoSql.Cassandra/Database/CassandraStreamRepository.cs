using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal sealed class CassandraStreamRepository : ICassandraStreamRepository
    {
        readonly ISession session;
        readonly CassandraStatementConfigurator configure;
        readonly Mapper mapper;
        readonly CassandraStorageConfiguration config;
        bool disposedValue;

        public CassandraStreamRepository(ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config, MappingConfiguration mapping)
        {
            session = sessionFactory.ThrowIfNull(nameof(session)).Open();
            this.config = config.ThrowIfNull(nameof(config));
            configure = new CassandraStatementConfigurator(config); 
            mapper = new Mapper(session, mapping.ThrowIfNull(nameof(mapping)));
        }


        public async Task<int> GetStreamActualRevision(Id streamId)
        {
            var cql = new Cql($"SELECT MAX(revision) FROM {config.EventsTableName} WHERE stream_id = ?", (string)streamId);
            var revision = await mapper.SingleAsync<int?>(cql);
            if (revision == null)
            {
                return Revision.Zero;
            }
            return revision.Value;
        }

        public async Task DeleteStream(Id streamId)
        {
            var cql = new Cql($"DELETE FROM {config.EventsTableName} WHERE stream_id = ?", (string)streamId);
            await mapper.ExecuteAsync(configure.Query(cql));
        }

        public async Task<IEnumerable<EventMetadataEntity>> FindMetadata(Id streamId)
        {
            var cql = new Cql($"SELECT id, stream_id, timestamp, revision  FROM {config.EventsTableName} WHERE stream_id = ?", (string)streamId);

            return await mapper.FetchAsync<EventMetadataEntity>(configure.Query(cql));
        }

        public async Task<AppliedInfo<EventEntity>> AppendToStream(Id streamId, params EventRecord[] records)
        {
                       
            var batch = configure.Batch(mapper.CreateBatch(BatchType.Logged));
              

            foreach (var record in records)
            {
                batch.InsertIfNotExists(record.ToEntity(streamId));
            }

            return await mapper.ExecuteConditionalAsync<EventEntity>(batch);
        }

        public async Task<IEnumerable<EventEntity>> GetEvents(Id streamId, Revision startFrom, int count)
        {
            var cql = new Cql($"SELECT *  FROM {config.EventsTableName} WHERE stream_id = ? AND revision >=? LIMIT ?", (string)streamId, (int)startFrom, count);

            return await mapper.FetchAsync<EventEntity>(configure.Query(cql));
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    session.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
