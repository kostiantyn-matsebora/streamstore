using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Mapping;
using StreamStore.Storage;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;
using Cassandra;
using StreamStore.Exceptions;
using System.Collections.Generic;
using StreamStore.Extensions;

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

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            await mapper.ExecuteAsync(configure.Query(queries.DeleteStream(streamId)));
        }

        protected override void BuildRecord(IStreamEventRecordBuilder builder, EventEntity entity)
        {
            builder
              .WithId(entity.Id)
              .Dated(entity.Timestamp)
              .WithRevision(entity.Revision)
              .WithData(entity.Data!)
              .WithCustomProperties(entity.CustomProperties);
        }

        protected override async Task<IStreamMetadata?> GetMetadataInternal(Id streamId, CancellationToken token = default)
        {
            var result = await mapper.FirstOrDefaultAsync<EventMetadataEntity>(configure.Query(queries.StreamMetadata(streamId)));
            if (result == null)
            {
                return null;
            }

            return new StreamMetadata(streamId, result.Revision, result.Timestamp);
        }
        
        protected override async Task<EventEntity[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
           return  (await mapper.FetchAsync<EventEntity>(configure.Query(queries.StreamEvents(streamId, startFrom, count)))).ToArray();
        }

        protected override async Task WriteAsyncInternal(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
        {
            var cqlBatch = configure.Batch(mapper.CreateBatch(BatchType.Logged));

            foreach (var record in batch)
            {
                cqlBatch.InsertIfNotExists(record.ToEntity(streamId));
            }

            var result = await mapper.ExecuteConditionalAsync<EventEntity>(cqlBatch);

            if (!result.Applied)
            {
                throw new RevisionAlreadyExistsException(result.Existing.Revision, streamId);
            }
        }
    }
}
