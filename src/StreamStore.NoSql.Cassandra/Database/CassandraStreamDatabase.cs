﻿using System;
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
        readonly CassandraStatementConfigurator configure;
        readonly ICassandraCqlQueries queries;
        readonly Lazy<ICassandraMapper> mapper;
        public CassandraStreamDatabase(ICassandraMapperProvider mapperProvider, ICassandraCqlQueries queries, CassandraStorageConfiguration config)
        {
            config = config.ThrowIfNull(nameof(config));
            configure = new CassandraStatementConfigurator(config);
            this.queries = queries.ThrowIfNull(nameof(queries));
            mapper = new Lazy<ICassandraMapper>(() => mapperProvider.OpenMapper());
        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult<IStreamUnitOfWork>(
                new CassandraStreamUnitOfWork(streamId, expectedStreamVersion, null, mapper.Value, configure, queries));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {

                await mapper.Value.ExecuteAsync(configure.Query(queries.DeleteStream(streamId)));
        }

        protected override async Task<Revision?> GetActualRevisionInternal(Id streamId, CancellationToken token = default)
        {
            return await mapper.Value.SingleOrDefaultAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
        }
        
        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
                var events = await mapper.Value.FetchAsync<EventEntity>(configure.Query(queries.StreamEvents(streamId, startFrom, count)));
                return events.ToArray().ToRecords();
        }
    }
}
