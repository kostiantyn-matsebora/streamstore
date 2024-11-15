using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Sqlite
{
    internal class SqliteProvisioningQueryProvider : ISqlProvisioningQueryProvider
    {
        readonly SqlDatabaseConfiguration configuration;

        public SqliteProvisioningQueryProvider(SqlDatabaseConfiguration configuration)
        {
          this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public string GetSchemaProvisioningQuery()
        {
            return @$"
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
                CREATE UNIQUE INDEX IF NOT EXISTS {configuration.SchemaName}.ix_streams_stream_id_revision ON {configuration.TableName}(StreamId, Revision);";
        }
    }
}
