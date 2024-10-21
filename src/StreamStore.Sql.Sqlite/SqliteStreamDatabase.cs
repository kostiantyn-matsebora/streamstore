using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper.Extensions;

namespace StreamStore.SQL.Sqlite
{
    public sealed class SqliteStreamDatabase : IStreamDatabase
    {
        private readonly IDapper dapper;
        readonly SqliteDatabaseConfiguration configuration;

        public SqliteStreamDatabase(IDapper dapper, SqliteDatabaseConfiguration configuration)
        {
            this.dapper = dapper ?? throw new ArgumentNullException(nameof(dapper));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult((IStreamUnitOfWork)new SqliteStreamUnitOfWork(streamId, expectedStreamVersion, null, configuration, dapper));
        }

        public async Task DeleteAsync(Id streamId, CancellationToken token = default)
        {
            var sql = $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
            using (var transaction = dapper.BeginTransaction())
            {
                await dapper.ExecuteAsync(sql, new { StreamId = streamId.Value });
                dapper.CommitTransaction();
            }
        }

        public async Task<StreamRecord?> FindAsync(Id streamId, CancellationToken token = default)
        {
            var sql = $"SELECT * FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
            EventEntity[] entities = await GetStreamEntities(streamId.Value, sql);

            if (entities == null || !entities.Any())
            {
                return null;
            }

            return new StreamRecord(entities.ToCollection());
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            var sql = $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
            EventEntity[] entities = await GetStreamEntities(streamId.Value, sql);

            if (entities == null || !entities.Any())
            {
                return null;
            }

            return new StreamMetadataRecord(entities.ToCollection());
        }

        public Task<EventRecord[]> ReadAsync(Id streamId, Revision start, int count, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        async Task<EventEntity[]> GetStreamEntities(string streamId, string sql)
        {
            IEnumerable<EventEntity> entities;
            entities = await dapper.QueryAsync<EventEntity>(sql, new { StreamId = streamId });
            return entities.ToArray();
        }
    }
}
