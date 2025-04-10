using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Mapping;
using StreamStore.Storage;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal class CassandraStreamStorage : StreamStorageBase<EventEntity>
    {
        readonly CassandraStatementConfigurator configure;
        readonly ICassandraCqlQueries queries;
        readonly IMapper mapper;
        public CassandraStreamStorage(ICassandraMapperProvider mapperProvider, ICassandraCqlQueries queries, CassandraStorageConfiguration config)
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
                new CassandraStreamWriter(streamId, expectedStreamVersion, null, mapper, configure, queries));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {

                await mapper.ExecuteAsync(configure.Query(queries.DeleteStream(streamId)));
        }

        protected override IStreamEventRecord ConvertToRecord(IStreamEventRecordBuilder builder, EventEntity entity)
        {
          return  builder
                    .WithId(entity.Id)
                    .Dated(entity.Timestamp)
                    .WithRevision(entity.Revision)
                    .WithData(entity.Data!)
                    .Build();
        }

        protected override async Task<Revision?> GetActualRevisionInternal(Id streamId, CancellationToken token = default)
        {
            return await mapper.SingleOrDefaultAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
        }
        
        protected override async Task<EventEntity[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
                return  (await mapper.FetchAsync<EventEntity>(configure.Query(queries.StreamEvents(streamId, startFrom, count)))).ToArray();
        }
    }
}
