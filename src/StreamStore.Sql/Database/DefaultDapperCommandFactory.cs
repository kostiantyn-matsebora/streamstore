﻿using System.Data;
using Dapper;
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
               commandText: queryProvider.GetQuery(SqlQueryType.AppendEvent),
               parameters: events,
               transaction: transaction);
        }

        public CommandDefinition CreateStreamDeleteCommand(Id streamId, IDbTransaction transaction)
        {
            return new CommandDefinition(
                commandText: queryProvider.GetQuery(SqlQueryType.DeleteStream),
                parameters: new { StreamId = streamId.Value },
                transaction: transaction);
        }

        public CommandDefinition CreateGetActualRevisionCommand(Id streamId)
        {
            return new CommandDefinition(
                 commandText: queryProvider.GetQuery(SqlQueryType.GetStreamActualRevision),
                 parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGetEventCountCommand(Id streamId)
        {
            return new CommandDefinition(
               commandText: queryProvider.GetQuery(SqlQueryType.GetStreamEventCount),
               parameters: new { StreamId = streamId.Value });
        }

        public CommandDefinition CreateGetEventsCommand(Id streamId, Revision startFrom, int count)
        {
            return new CommandDefinition(
              commandText: queryProvider.GetQuery(SqlQueryType.GetEvents),
              parameters: new { StreamId = (string)streamId, Revision = (int)startFrom, Count = count });
        }

        public CommandDefinition CreateGetStreamMetadataCommand(Id streamId)
        {
            return new CommandDefinition(
              commandText: queryProvider.GetQuery(SqlQueryType.GetStreamMetadata),
              parameters: new { StreamId = streamId.Value });
        }

    }
}