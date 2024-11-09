
using Dapper;
using System.Data;

namespace StreamStore.Sql.API
{
    public interface IDapperCommandFactory
    {
        CommandDefinition CreateDeletionCommand(Id streamId, IDbTransaction transaction);
        CommandDefinition CreateGettingMetadataCommand(Id streamId);

        CommandDefinition CreateGettingActualRevisionCommand(Id streamId);

        CommandDefinition CreateGettingEventCountCommand(Id streamId);

        CommandDefinition CreateGettingEventsCommand(Id streamId, Revision startFrom, int count);

        CommandDefinition CreateAppendingEventsCommand(Id streamId, EventEntity[] events, IDbTransaction transaction);

        CommandDefinition CreateSchemaProvisioningCommand();
    }
}
