
using Dapper;
using System.Data;

namespace StreamStore.Sql.API
{
    public interface IDapperCommandFactory
    {
        CommandDefinition CreateStreamDeleteCommand(Id streamId, IDbTransaction transaction);
        CommandDefinition CreateGetStreamMetadataCommand(Id streamId);

        CommandDefinition CreateGetActualRevisionCommand(Id streamId);

        CommandDefinition CreateGetEventCountCommand(Id streamId);

        CommandDefinition CreateGetEventsCommand(Id streamId, Revision startFrom, int count);

        CommandDefinition CreateAppendEventCommand(Id streamId, EventEntity[] events, IDbTransaction transaction);
    }
}
