using System;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.Exceptions;


namespace StreamStore.SQL.Sqlite
{
    internal class SqliteStreamUnitOfWork : StreamUnitOfWorkBase
    {
        readonly SqliteDatabaseConfiguration configuration;
        readonly IDbConnectionFactory connectionFactory;

        public SqliteStreamUnitOfWork(Id streamId, Revision expectedRevision, StreamRecord? existing, SqliteDatabaseConfiguration configuration, IDbConnectionFactory connectionFactory) :
            base(streamId, expectedRevision, existing)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            var sql = $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data)";
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);

                using (var trransaction = await connection.BeginTransactionAsync(token))
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, uncommited.ToEntityArray(streamId));
                        await trransaction.CommitAsync(token);
                    }
                    catch (SQLiteException e)
                    {
                        if (e.ErrorCode != 19)
                        {
                            throw;
                        }

                        throw new OptimisticConcurrencyException(expectedRevision, GetActualRevision(), streamId);
                    }
                }
            }
        }

        int GetActualRevision()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                var sql = $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
                return connection.ExecuteScalar<int>(sql, new { StreamId = (string)streamId });
            }
        }
    }
}
