using System.Data;
using Dapper;
using StreamStore.Storage;
using StreamStore.Sql.API;


namespace StreamStore.Sql.Database
{
    internal class DefaultDapperCommandFactory: IDapperCommandFactory
    {
        readonly ISqlQueryProvider queryProvider;

        public DefaultDapperCommandFactory(ISqlQueryProvider queryProvider)
        {
            this.queryProvider = queryProvider.ThrowIfNull(nameof(queryProvider));
        }

        public CommandDefinition CreateAppendEventCommand(Id streamId, EventEntity[] events, IDbTransaction transaction)
        {
            return new CommandDefinition(
               commandText: queryProvider.GetQuery(StorageOperation.AppendEvent),
               parameters: events,
               transaction: transaction);
        }

        public CommandDefinition CreateStreamDeleteCommand(Id streamId, IDbTransaction transaction)
        {
            return new CommandDefinition(
                commandText: queryProvider.GetQuery(StorageOperation.DeleteStream),
                parameters: new { StreamId = streamId.Value },
                transaction: transaction);
        }

        public CommandDefinition CreateGetActualRevisionCommand(Id streamId)
        {
            return new CommandDefinition(
                 commandText: queryProvider.GetQuery(StorageOperation.GetStreamActualRevision),
                 parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGetEventCountCommand(Id streamId)
        {
            return new CommandDefinition(
               commandText: queryProvider.GetQuery(StorageOperation.GetStreamEventCount),
               parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGetEventsCommand(Id streamId, Revision startFrom, int count)
        {
            return new CommandDefinition(
              commandText: queryProvider.GetQuery(StorageOperation.GetEvents),
              parameters: new { StreamId = (string)streamId, Revision = (int)startFrom, Count = count });
        }

        public CommandDefinition CreateGetStreamMetadataCommand(Id streamId)
        {
            return new CommandDefinition(
              commandText: queryProvider.GetQuery(StorageOperation.GetStreamMetadata),
              parameters: new { StreamId = streamId.Value });
        }

    }
}
