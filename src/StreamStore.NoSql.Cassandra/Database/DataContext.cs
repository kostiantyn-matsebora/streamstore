using System;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class DataContext: IDisposable
    {
        readonly CassandraStorageConfiguration config;
        readonly Table<EventEntity> events;
        readonly Table<EventMetadataEntity> metadata;
        readonly Table<RevisionStreamEntity> streamRevisions;
        readonly ISession session;
        private bool disposedValue;

        public DataContext(TypeMapFactory mapper, ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config)
        {
            session = sessionFactory.ThrowIfNull(nameof(session)).Open();
            this.config = config.ThrowIfNull(nameof(config));
            events = CreateTable(session, mapper.CreateEventEntityMap());
            metadata = CreateTable(session, mapper.CreateEventMetadataMap());
            streamRevisions = CreateTable(session, mapper.CreateStreamRevisionMap());
        }

        public CqlQueryBase<int> GetStreamRevisions(Id streamId)
        {
            var id = (string)streamId;

            return ConfigureQuery(
                  streamRevisions
                      .Where(er => er.StreamId == id)
                      .Select(er => er.Revision));
        }

        public CqlQueryBase<EventEntity> DeleteStream(Id streamId)
        {
            var id = (string)streamId;
            return events.Where(er => er.StreamId == id);
        }

        public CqlQueryBase<EventMetadataEntity> FindMetadata(Id streamId)
        {
            var id = (string)streamId;
            return ConfigureQuery(metadata.Where(er => er.StreamId == id));
        }

        public CqlCommand AppendToStream(Id streamId, EventRecord record)
        {
            return
                ConfigureInsert(
                    events.Insert(record.ToEntity(streamId)).IfNotExists());
        }

        public CqlQueryBase<EventEntity> GetEvents(Id streamId, Revision startFrom, int count)
        {
            string id = (string)streamId;
            int revision = (int)startFrom;
            return ConfigureQuery(events.Where(er => er.StreamId == id && er.Revision >= revision).Take(count));
        }

        public async Task CreateIfNotExistsAsync()
        {
            await events.CreateIfNotExistsAsync();
        }

        Table<T> CreateTable<T>(ISession session, Map<T> map)
        {
            return new Table<T>(session, new MappingConfiguration().Define(map));
        }


        CqlQuery<TEntity> ConfigureQuery<TEntity>(CqlQuery<TEntity> query)
        {
            return query
                .SetConsistencyLevel(config.ReadConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
        }

        CqlCommand ConfigureInsert(CqlCommand command)
        {
            return command
                .SetConsistencyLevel(config.WriteConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
        }

        protected virtual void Dispose(bool disposing)
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

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DataContext()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
