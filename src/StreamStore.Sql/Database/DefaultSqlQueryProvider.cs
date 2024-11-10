using System;
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

        public string GetQuery(SqlQueryType queryType) => queryType switch
        {
            SqlQueryType.DeleteStream => DeleteStream,
            SqlQueryType.AppendEvent => AppendEvent,
            SqlQueryType.GetEvents => GetEvents,
            SqlQueryType.GetStreamActualRevision => GetStreamActualRevision,
            SqlQueryType.GetStreamEventCount => GetStreamEventCount,
            SqlQueryType.GetStreamMetadata => GetStreamMetadata,
            _ => throw new ArgumentOutOfRangeException(nameof(queryType), $"Not expected query type value: {queryType}"),
        };

        string GetStreamMetadata => $"SELECT Id, Revision, Timestamp FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

        string GetStreamEventCount => $"SELECT COUNT(Id)  FROM {configuration.FullTableName} WHERE StreamId = @StreamId";


        string GetEvents => $"SELECT Id, Revision, Timestamp, Data FROM {configuration.FullTableName} WHERE StreamId = @StreamId and Revision >= @Revision ORDER BY Revision ASC LIMIT @Count";
    
        string DeleteStream => $"DELETE FROM {configuration.FullTableName} WHERE StreamId = @StreamId";

        string AppendEvent => $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data)";

        string GetStreamActualRevision => $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId";


    }
}
