using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal sealed class CassandraStreamRepository : ICassandraStreamRepository
    {

        readonly Table<EventEntity> events;
        readonly Table<EventMetadataEntity> metadata;
        readonly Table<RevisionStreamEntity> streamRevisions;
        readonly ISession session;
        readonly MappingConfiguration mappingConfig;
        readonly CassandraStatementConfigurator queryConfigurator;
        bool disposedValue;

        public CassandraStreamRepository(ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config)
        {
            session = sessionFactory.ThrowIfNull(nameof(session)).Open();
            queryConfigurator = new CassandraStatementConfigurator(config);
            mappingConfig = ConfigureMapping(config);
            events = CreateTable<EventEntity>(session);
            metadata = CreateTable<EventMetadataEntity>(session);
            streamRevisions = CreateTable<RevisionStreamEntity>(session);
        }


        public async Task<int> GetStreamActualRevision(Id streamId)
        {
            var id = (string)streamId;

            var revisions = (
                await queryConfigurator.ConfigureQuery<CqlQuery<int>>(
                streamRevisions
                      .Where(er => er.StreamId == id)
                      .Select(er => er.Revision))
                .ExecuteAsync()).ToArray();

            if (!revisions.Any())
            {
                return Revision.Zero;
            }
            return revisions.Max();
        }

        public async Task DeleteStream(Id streamId)
        {
            var id = (string)streamId;
            await events.Where(er => er.StreamId == id).Delete().ExecuteAsync();
        }

        public async Task<IEnumerable<EventMetadataEntity>> FindMetadata(Id streamId)
        {
            var id = (string)streamId;
            return await queryConfigurator.ConfigureQuery<CqlQuery<EventMetadataEntity>>(metadata.Where(er => er.StreamId == id)).ExecuteAsync();
        }

        public async Task<AppliedInfo<EventEntity>> AppendToStream(Id streamId, params EventRecord[] records)
        {
            var mapper = new Mapper(session, mappingConfig);
            var batch = mapper.CreateBatch(BatchType.Logged);
            var options = queryConfigurator.ConfigureInsert(new CqlQueryOptions());

            foreach (var record in records)
            {
                batch.InsertIfNotExists(record.ToEntity(streamId), options);
            }

            return await mapper.ExecuteConditionalAsync<EventEntity>(batch);
        }

        public async Task<IEnumerable<EventEntity>> GetEvents(Id streamId, Revision startFrom, int count)
        {
            string id = (string)streamId;
            int revision = (int)startFrom;

            return await queryConfigurator.ConfigureQuery<CqlQuery<EventEntity>>(
                    events
                        .Where(er => er.StreamId == id && er.Revision >= revision)
                        .Take(count))
                .ExecuteAsync();
        }

        public async Task CreateSchemaIfNotExistsAsync()
        {
            await events.CreateIfNotExistsAsync();
        }

        static MappingConfiguration ConfigureMapping(CassandraStorageConfiguration config)
        {
            return new MappingConfiguration().Define(new CassandraStreamMapping(config));
        }

        Table<T> CreateTable<T>(ISession session)
        {
            return new Table<T>(session, mappingConfig);
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
