using System.Data;
using Dapper;
using StreamStore.SQL;

namespace StreamStore.Sql.Sqlite
{
    internal class SqliteDapperCommandFactory : IDapperCommandFactory
    {
        readonly SqlDatabaseConfiguration configuration;

        public SqliteDapperCommandFactory(SqlDatabaseConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public CommandDefinition CreateAppendingEventsCommand(Id streamId, EventEntity[] events, IDbTransaction transaction)
        {
            return new CommandDefinition(
               commandText: $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data)",
               parameters: events,
               transaction: transaction);
        }

        public CommandDefinition CreateDeletionCommand(Id streamId, IDbTransaction transaction)
        {
            return new CommandDefinition(
                commandText: $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId", 
                parameters: new { StreamId = streamId.Value }, 
                transaction: transaction);
        }

        public CommandDefinition CreateGettingActualRevisionCommand(Id streamId)
        {
            return new CommandDefinition(
                 commandText: $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
                 parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGettingEventCountCommand(Id streamId)
        {
            return new CommandDefinition(
               commandText: $"SELECT COUNT(Id)  FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
               parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGettingEventsCommand(Id streamId, Revision startFrom, int count)
        {
            return new CommandDefinition(
              commandText: $"SELECT Id, Revision, Timestamp, Data FROM {configuration.FullTableName} WHERE StreamId = @StreamId and Revision >= @Revision ORDER BY Revision ASC LIMIT @Count",
              parameters: new { StreamId = (string)streamId, Revision = (int)startFrom, Count = count });
        }

        public CommandDefinition CreateGettingMetadataCommand(Id streamId)
        {
            return new CommandDefinition(
              commandText: $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId",
              parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateSchemaProvisioningCommand()
        {
            return new CommandDefinition(
               commandText: @$"
                CREATE TABLE IF NOT EXISTS {configuration.FullTableName} (
                    Id TEXT NOT NULL,
                    StreamId TEXT NOT NULL,
                    Timestamp datetime2 NOT NULL, 
                    Revision INTEGER NOT NULL,
                    Data BLOB NOT NULL,
                    PRIMARY KEY (Id, StreamId)
                );

                CREATE INDEX IF NOT EXISTS {configuration.SchemaName}.ix_streams_stream_id ON {configuration.TableName}(StreamId);
                CREATE INDEX IF NOT EXISTS {configuration.SchemaName}.ix_streams_stream_revision ON {configuration.TableName}(Revision);
                CREATE UNIQUE INDEX IF NOT EXISTS {configuration.SchemaName}.ix_streams_stream_id_revision ON {configuration.TableName}(StreamId, Revision);"
            );
        }
    }
}
