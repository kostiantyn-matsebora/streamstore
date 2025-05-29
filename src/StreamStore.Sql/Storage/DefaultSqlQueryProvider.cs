using System;
using StreamStore.Storage;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Storage
{
    internal  class DefaultSqlQueryProvider : ISqlQueryProvider
    {
        readonly SqlStorageConfiguration configuration;

        public DefaultSqlQueryProvider(SqlStorageConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public string GetQuery(StorageOperation operation) => operation switch
        {
            StorageOperation.DeleteStream => DeleteStream,
            StorageOperation.AppendEvent => AppendEvent,
            StorageOperation.GetEvents => GetEvents,
            StorageOperation.GetStreamMetadata => GetStreamMetadata,
            StorageOperation.GetStreamEventCount => GetStreamEventCount,
            StorageOperation.GetStreamEventsMetadata => GetStreamEventsMetadata,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Not expected query type value: {operation}"),
        };

        string GetStreamEventsMetadata => $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

        string GetStreamEventCount => $"SELECT COUNT(Id)  FROM {configuration.FullTableName} WHERE StreamId = @StreamId";


        string GetEvents => $"SELECT Id, Revision, Timestamp, Data, CustomProperties FROM {configuration.FullTableName} WHERE StreamId = @StreamId and Revision >= @Revision ORDER BY Revision ASC LIMIT @Count";
    
        string DeleteStream => $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

        string AppendEvent => $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data, CustomProperties) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data, @CustomProperties)";

        string GetStreamMetadata => $"SELECT  Id, Revision, Timestamp, StreamId FROM {configuration.FullTableName} WHERE StreamId = @StreamId ORDER BY Revision DESC LIMIT 1";
    }
}
