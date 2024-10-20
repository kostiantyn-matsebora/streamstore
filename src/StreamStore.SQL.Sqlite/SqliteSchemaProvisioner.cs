using System;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Extensions;


namespace StreamStore.SQL.Sqlite
{
    internal sealed class SqliteSchemaProvisioner
    {
        readonly SqliteDatabaseConfiguration configuration;
        readonly IDapper dapper;

        public SqliteSchemaProvisioner(SqliteDatabaseConfiguration configuration, IDapper dapper) {
            
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.dapper = dapper ?? throw new ArgumentNullException(nameof(dapper));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            string query = @$"
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

          await dapper.ExecuteAsync(query);
        }
    }
}
