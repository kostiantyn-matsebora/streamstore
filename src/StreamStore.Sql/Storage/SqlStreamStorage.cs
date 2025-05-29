using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using StreamStore.Exceptions;
using StreamStore.Sql.API;
using StreamStore.Storage;


namespace StreamStore.Sql.Storage
{
    public class SqlStreamStorage : StreamStorageBase<EventEntity>
    {
        readonly IDbConnectionFactory connectionFactory;
        readonly IDapperCommandFactory commandFactory;
        readonly ISqlExceptionHandler exceptionHandler;
        const string EmptyJson = "{}";

        public SqlStreamStorage(IDbConnectionFactory connectionFactory, IDapperCommandFactory commandFactory, ISqlExceptionHandler exceptionHandler)
        {
            this.connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
            this.commandFactory = commandFactory.ThrowIfNull(nameof(commandFactory));
            this.exceptionHandler = exceptionHandler.ThrowIfNull(nameof(exceptionHandler));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    await connection.ExecuteAsync(commandFactory.CreateStreamDeleteCommand(streamId, transaction));
                    await transaction.CommitAsync(token);
                }
            }
        }

        protected override async Task<IStreamMetadata?> GetMetadataInternal(Id streamId, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                var result = await connection.QueryFirstOrDefaultAsync<EventMetadataEntity>(commandFactory.CreateGetMetadataCommand(streamId));
                if (result == null)
                {
                    return null;
                }
                return new StreamMetadata(streamId, result.Revision, result.Timestamp);
            }
        }

        protected override async Task<EventEntity[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);
                var number = await connection.ExecuteScalarAsync<int>(commandFactory.CreateGetEventCountCommand(streamId));

                if (number == 0)
                    throw new StreamNotFoundException(streamId);

               return (await connection.QueryAsync<EventEntity>(commandFactory.CreateGetEventsCommand(streamId, startFrom, count))).ToArray();
            }
        }

        protected override void BuildRecord(IStreamEventRecordBuilder builder, EventEntity entity)
        {
            builder
               .WithId(entity.Id!)
               .WithRevision(entity.Revision!)
               .Dated(entity.Timestamp)
               .WithData(entity.Data!);
            if (entity.CustomProperties != null && entity.CustomProperties != EmptyJson)
                builder.WithCustomProperties(
                    JsonSerializer.Deserialize<IReadOnlyDictionary<string, string>>(entity.CustomProperties)!);
        }

        protected override async Task WriteAsyncInternal(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
        {
            using (var connection = connectionFactory.GetConnection())
            {
                await connection.OpenAsync(token);

                using (var transaction = await connection.BeginTransactionAsync(token))
                {
                    try
                    {
                        await connection.ExecuteAsync(commandFactory.CreateAppendEventCommand(streamId, batch.ToEntityArray(streamId), transaction));
                        await transaction.CommitAsync(token);
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler.HandleException(ex, streamId);
                        throw;
                    }
                }
            }
        }
    }
}
