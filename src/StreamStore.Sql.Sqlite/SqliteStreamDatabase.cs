using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.Database;
using StreamStore.Exceptions;

namespace StreamStore.SQL.Sqlite
{
    public sealed class SqliteStreamDatabase : StreamDatabaseBase
    {
        readonly IDbConnectionFactory connectionFactory;
        readonly SqliteDatabaseConfiguration configuration;

        public SqliteStreamDatabase(IDbConnectionFactory connectionFactory, SqliteDatabaseConfiguration configuration)
        {
            this.connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult((IStreamUnitOfWork)
                new SqliteStreamUnitOfWork(streamId, expectedStreamVersion, null, configuration, connectionFactory));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            var sql = $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    await connection.ExecuteAsync(sql, new { StreamId = streamId.Value });
                    await transaction.CommitAsync(token);
                }
            }

        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                var sql = $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
                EventEntity[] entities = await GetStreamEntities(new { StreamId = (string)streamId }, sql, connection);

                if (entities == null || !entities.Any())
                {
                    return null;
                }

                return new EventMetadataRecordCollection(entities.ToRecords());
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            var sql = $"SELECT COUNT(Id)  FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                var number = await connection.ExecuteScalarAsync<int>(sql, new { StreamId = streamId.Value });

                if (number == 0)
                    throw new StreamNotFoundException(streamId);

                sql = $"SELECT Id, Revision, Timestamp, Data FROM {configuration.FullTableName} WHERE StreamId = @StreamId and Revision >= @Revision ORDER BY Revision ASC LIMIT @Count";

                var entities = await GetStreamEntities(
                    new { StreamId = (string)streamId, Revision = (int)startFrom, Count = count }, sql, connection);


                return entities.ToArray().ToRecords();
            }
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                var sql = $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
                return await connection.ExecuteScalarAsync<int>(sql, new { StreamId = (string)streamId });
            }
        }

        static async Task<EventEntity[]> GetStreamEntities(object parameters, string sql, DbConnection connection)
        {

            IEnumerable<EventEntity> entities;
            entities = await connection.QueryAsync<EventEntity>(sql, parameters);
            return entities.ToArray();
        }


    }
}
