using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Mapping;
using StreamStore.Storage;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamDatabase : StreamDatabaseBase
    {
        readonly CassandraStatementConfigurator configure;
        readonly ICassandraCqlQueries queries;
        readonly IMapper mapper;
        public CassandraStreamDatabase(ICassandraMapperProvider mapperProvider, ICassandraCqlQueries queries, CassandraStorageConfiguration config)
        {
            config = config.ThrowIfNull(nameof(config));
            configure = new CassandraStatementConfigurator(config);
            this.queries = queries.ThrowIfNull(nameof(queries));
            mapperProvider.ThrowIfNull(nameof(mapperProvider));
            mapper = mapperProvider.OpenMapper();
        }

        protected override Task<IStreamWriter> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult<IStreamWriter>(
                new CassandraStreamUnitOfWork(streamId, expectedStreamVersion, null, mapper, configure, queries));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {

                await mapper.ExecuteAsync(configure.Query(queries.DeleteStream(streamId)));
        }

        protected override async Task<Revision?> GetActualRevisionInternal(Id streamId, CancellationToken token = default)
        {
            return await mapper.SingleOrDefaultAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
        }
        
        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
                var events = await mapper.FetchAsync<EventEntity>(configure.Query(queries.StreamEvents(streamId, startFrom, count)));
                return events.ToArray().ToRecords();
        }
    }
}
