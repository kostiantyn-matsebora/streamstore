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
        readonly ICassandraCqlQueries queries;

        public CassandraStreamDatabase(ICassandraMapperProvider mapperProvider, ICassandraCqlQueries queries, CassandraStorageConfiguration config)
        {
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            config = config.ThrowIfNull(nameof(config));
            configure = new CassandraStatementConfigurator(config);
            this.queries = queries.ThrowIfNull(nameof(queries));
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

        protected override async Task<Revision?> GetActualRevisionInternal(Id streamId, CancellationToken token = default)
        {
            using (var mapper = mapperProvider.OpenMapper())
            {
               return await mapper.SingleOrDefaultAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
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
