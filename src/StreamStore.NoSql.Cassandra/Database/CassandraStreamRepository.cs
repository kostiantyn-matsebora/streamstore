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
        readonly CassandraCqlQueries queries;
        bool disposedValue;

        public CassandraStreamRepository(ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config, MappingConfiguration mapping)
        {
            session = sessionFactory.ThrowIfNull(nameof(session)).Open();
            configure = new CassandraStatementConfigurator(config); 
            mapper = new Mapper(session, mapping.ThrowIfNull(nameof(mapping)));
            queries = new CassandraCqlQueries(config);
        }


        public async Task<int> GetStreamActualRevision(Id streamId)
        {
            var revision = await mapper.SingleAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
            if (revision == null)
            {
                return Revision.Zero;
            }
            return revision.Value;
        }

        public async Task DeleteStream(Id streamId)
        {
            await mapper.ExecuteAsync(configure.Query(queries.DeleteStream(streamId)));
        }

        public async Task<IEnumerable<EventMetadataEntity>> FindMetadata(Id streamId)
        {
            return await mapper.FetchAsync<EventMetadataEntity>(configure.Query(queries.StreamMetadata(streamId)));
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
            return await mapper.FetchAsync<EventEntity>(configure.Query(queries.StreamEvents(streamId, startFrom, count)));
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
