using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.Exceptions;
using StreamStore.Sql.API;
using StreamStore.Storage;


namespace StreamStore.Sql.Storage
{
    public class SqlStreamWriter : StreamWriterBase
    {
        readonly IDbConnectionFactory connectionFactory;
        readonly IDapperCommandFactory commandFactory;
        readonly ISqlExceptionHandler exceptionHandler;

        public SqlStreamWriter(Id streamId, Revision expectedRevision, EventRecordCollection? existing, IDbConnectionFactory connectionFactory, IDapperCommandFactory commandFactory, ISqlExceptionHandler exceptionHandler) :
            base(streamId, expectedRevision, existing)
        {
            this.connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
            this.commandFactory = commandFactory.ThrowIfNull(nameof(commandFactory));
            this.exceptionHandler = exceptionHandler.ThrowIfNull(nameof(exceptionHandler));
        }

        protected override async Task CommitAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);

                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    try
                    {
                        await connection.ExecuteAsync(commandFactory.CreateAppendEventCommand(streamId, uncommited.ToEntityArray(streamId), transaction));
                        await transaction.CommitAsync(token);
                    }
                    catch (Exception ex)
                    {
                        if (exceptionHandler.IsOptimisticConcurrencyException(ex))
                        {
                            throw new OptimisticConcurrencyException(expectedRevision, GetActualRevision(), streamId);
                        }

                        throw;
                    }
                }
            }
        }
        int GetActualRevision()
        {
            using (var connection = connectionFactory.GetConnection())
            {
                return connection.ExecuteScalar<int>(commandFactory.CreateGetActualRevisionCommand(streamId));
            }
        }
    }
}
