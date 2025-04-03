using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresProvisioningQueryProvider : ISqlProvisioningQueryProvider
    {
        readonly SqlStorageConfiguration configuration;

        public PostgresProvisioningQueryProvider(SqlStorageConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public string GetSchemaProvisioningQuery()
        {
            return @$"
                CREATE TABLE IF NOT EXISTS {configuration.FullTableName} (
                    Id text NOT NULL,
                    StreamId text NOT NULL,
                    Timestamp timestamp NOT NULL,
                    Revision integer NOT NULL,
                    Data bytea NOT NULL,
                    PRIMARY KEY (Id, StreamId)
                );

                CREATE INDEX IF NOT EXISTS ix_events_stream_id ON {configuration.TableName}(StreamId);
                CREATE INDEX IF NOT EXISTS ix_events_stream_revision ON {configuration.TableName}(Revision);
                CREATE UNIQUE INDEX IF NOT EXISTS ix_events_stream_id_revision ON {configuration.TableName}(StreamId, Revision);";
        }
    }
}
