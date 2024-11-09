using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.Database;
using StreamStore.Exceptions;
using StreamStore.Sql.API;


namespace StreamStore.Sql.Database
{
    public class SqlStreamDatabase : StreamDatabaseBase
    {
        readonly IDbConnectionFactory connectionFactory;
        readonly IDapperCommandFactory commandFactory;
        readonly ISqlExceptionHandler exceptionHandler;

        public SqlStreamDatabase(IDbConnectionFactory connectionFactory, IDapperCommandFactory commandFactory, ISqlExceptionHandler exceptionHandler)
        {
            this.connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
            this.commandFactory = commandFactory.ThrowIfNull(nameof(commandFactory));
            this.exceptionHandler = exceptionHandler.ThrowIfNull(nameof(exceptionHandler));
        }

        protected override Task<IStreamUnitOfWork> BeginAppendAsyncInternal(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult((IStreamUnitOfWork)
                new SqlStreamUnitOfWork(streamId, expectedStreamVersion, null, connectionFactory, commandFactory, exceptionHandler));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    await connection.ExecuteAsync(commandFactory.CreateDeletionCommand(streamId, transaction));
                    await transaction.CommitAsync(token);
                }
            }
        }

        protected override async Task<EventMetadataRecordCollection?> FindMetadataAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);

                EventEntity[] entities = (await connection.QueryAsync<EventEntity>(commandFactory.CreateGettingMetadataCommand(streamId))).ToArray();

                if (!entities.Any())
                {
                    return null;
                }

                return new EventMetadataRecordCollection(entities.ToRecords());
            }
        }

        protected override async Task<int> GetActualRevision(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                return await connection.ExecuteScalarAsync<int>(commandFactory.CreateGettingActualRevisionCommand(streamId));
            }
        }

        protected override async Task<EventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                var number = await connection.ExecuteScalarAsync<int>(commandFactory.CreateGettingEventCountCommand(streamId));

                if (number == 0)
                    throw new StreamNotFoundException(streamId);

                var entities = (await connection.QueryAsync<EventEntity>(commandFactory.CreateGettingEventsCommand(streamId, startFrom, count))).ToArray();

                return entities.ToArray().ToRecords();
            }
        }
    }
}
