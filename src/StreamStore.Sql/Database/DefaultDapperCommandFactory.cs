using System.Data;
using Dapper;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Database
{
    internal class DefaultDapperCommandFactory: IDapperCommandFactory
    {
        readonly SqlDatabaseConfiguration configuration;

        public DefaultDapperCommandFactory(SqlDatabaseConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public CommandDefinition CreateAppendEventCommand(Id streamId, EventEntity[] events, IDbTransaction transaction)
        {
            return new CommandDefinition(
               commandText: $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data)",
               parameters: events,
               transaction: transaction);
        }

        public CommandDefinition CreateStreamDeleteCommand(Id streamId, IDbTransaction transaction)
        {
            return new CommandDefinition(
                commandText: $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
                parameters: new { StreamId = streamId.Value },
                transaction: transaction);
        }

        public CommandDefinition CreateGetActualRevisionCommand(Id streamId)
        {
            return new CommandDefinition(
                 commandText: $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
                 parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGetEventCountCommand(Id streamId)
        {
            return new CommandDefinition(
               commandText: $"SELECT COUNT(Id)  FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
               parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGetEventsCommand(Id streamId, Revision startFrom, int count)
        {
            return new CommandDefinition(
              commandText: $"SELECT Id, Revision, Timestamp, Data FROM {configuration.FullTableName} WHERE StreamId = @StreamId and Revision >= @Revision ORDER BY Revision ASC LIMIT @Count",
              parameters: new { StreamId = (string)streamId, Revision = (int)startFrom, Count = count });
        }

        public CommandDefinition CreateGetStreamMetadataCommand(Id streamId)
        {
            return new CommandDefinition(
              commandText: $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
              parameters: new { StreamId = streamId.Value });
        }

    }
}
