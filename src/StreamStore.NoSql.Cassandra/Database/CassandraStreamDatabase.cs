using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Database;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamDatabase : StreamDatabaseBase
    {
        readonly ICassandraMapperProvider mapperProvider;
        readonly CassandraStatementConfigurator configure;
        readonly CassandraCqlQueries queries;

        public CassandraStreamDatabase(ICassandraMapperProvider mapperProvider, CassandraStorageConfiguration config)
        {
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            config = config.ThrowIfNull(nameof(config));
            configure = new CassandraStatementConfigurator(config);
            queries = new CassandraCqlQueries(config);
        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult<IStreamUnitOfWork>(
                new CassandraStreamUnitOfWork(streamId, expectedStreamVersion, null, mapperProvider, configure, queries));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var mapper = mapperProvider.OpenMapper())
            {
                await mapper.ExecuteAsync(configure.Query(queries.DeleteStream(streamId)));
            }
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var mapper = mapperProvider.OpenMapper())
            {
                var metadata = await mapper.FetchAsync<EventMetadataEntity>(configure.Query(queries.StreamMetadata(streamId)));

                if (!metadata.Any())
                {
                    return null;
                }

                return new EventMetadataRecordCollection(metadata.ToArray().ToRecords());
            }
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var mapper = mapperProvider.OpenMapper())
            {
                var revision = await mapper.SingleAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
                if (revision == null)
                {
                    return Revision.Zero;
                }
                return revision.Value;
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var mapper = mapperProvider.OpenMapper())
            {
                var events = await mapper.FetchAsync<EventEntity>(configure.Query(queries.StreamEvents(streamId, startFrom, count)));
                return events.ToArray().ToRecords();
            }
        }
    }
}
