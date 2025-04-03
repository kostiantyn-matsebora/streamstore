using System;
using StreamStore.Storage;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Database
{
    internal  class DefaultSqlQueryProvider : ISqlQueryProvider
    {
        readonly SqlDatabaseConfiguration configuration;

        public DefaultSqlQueryProvider(SqlDatabaseConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public string GetQuery(StorageOperation operation) => operation switch
        {
            StorageOperation.DeleteStream => DeleteStream,
            StorageOperation.AppendEvent => AppendEvent,
            StorageOperation.GetEvents => GetEvents,
            StorageOperation.GetStreamActualRevision => GetStreamActualRevision,
            StorageOperation.GetStreamEventCount => GetStreamEventCount,
            StorageOperation.GetStreamMetadata => GetStreamMetadata,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Not expected query type value: {operation}"),
        };

        string GetStreamMetadata => $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

        string GetStreamEventCount => $"SELECT COUNT(Id)  FROM {configuration.FullTableName} WHERE StreamId = @StreamId";


        string GetEvents => $"SELECT Id, Revision, Timestamp, Data FROM {configuration.FullTableName} WHERE StreamId = @StreamId and Revision >= @Revision ORDER BY Revision ASC LIMIT @Count";
    
        string DeleteStream => $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

        string AppendEvent => $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data)";

        string GetStreamActualRevision => $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
    }
}
