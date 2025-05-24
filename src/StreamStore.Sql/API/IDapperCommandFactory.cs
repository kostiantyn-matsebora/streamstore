
using Dapper;
using System.Data;

namespace StreamStore.Sql.API
{
    public interface IDapperCommandFactory
    {
        CommandDefinition CreateStreamDeleteCommand(Id streamId, IDbTransaction transaction);
        CommandDefinition CreateGetStreamEventsMetadataCommand(Id streamId);

        CommandDefinition CreateGetMetadataCommand(Id streamId);

        CommandDefinition CreateGetEventCountCommand(Id streamId);

        CommandDefinition CreateGetEventsCommand(Id streamId, Revision startFrom, int count);

        CommandDefinition CreateAppendEventCommand(Id streamId, EventEntity[] events, IDbTransaction transaction);
    }
}
