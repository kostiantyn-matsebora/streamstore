﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using StreamStore.Exceptions;

using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly IMapper mapper;
        readonly CassandraStatementConfigurator configure;
        readonly ICassandraCqlQueries queries;


        public CassandraStreamUnitOfWork(
            Id streamId, 
            Revision expectedRevision, 
            EventRecordCollection? events,
            IMapper mapper,
            CassandraStatementConfigurator configure,
            ICassandraCqlQueries queries)
            : base(streamId, expectedRevision, events)
        {
            this.mapper = mapper.ThrowIfNull(nameof(mapper));
            this.configure = configure.ThrowIfNull(nameof(configure));
            this.queries = queries.ThrowIfNull(nameof(queries));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            var records = uncommited.ToArray();
            var batch = configure.Batch(mapper.CreateBatch(BatchType.Logged));


            foreach (var record in records)
            {
                batch.InsertIfNotExists(record.ToEntity(streamId));
            }

            var result = await mapper.ExecuteConditionalAsync<EventEntity>(batch);
            await ValidateResult(mapper, result);
        }

        async Task ValidateResult(IMapper mapper, AppliedInfo<EventEntity> appliedInfo)
        {
            if (appliedInfo.Applied)
            {
                return;
            }
            var actualRevision = await GetActualRevision(mapper);
            if (actualRevision != expectedRevision)
            {
                throw new OptimisticConcurrencyException(expectedRevision, actualRevision, streamId);
            }
        }

        async Task<int> GetActualRevision(IMapper mapper)
        {
            var revision = await mapper.SingleAsync<int?>(configure.Query(queries.StreamActualRevision(streamId)));
            if (revision == null)
            {
                return Revision.Zero;
            }
            return revision.Value;

        }
    }
}
