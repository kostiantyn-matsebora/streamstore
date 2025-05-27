using FluentMigrator;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Migrations
{
    [Migration(1)]
    public class Initial : Migration
    {

        readonly SqlStorageConfiguration configuration;

        public Initial(SqlStorageConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }
       

        public override void Up()
        {
            // Since FluentMigrator does not support if exists for table creation, we will SQL statement to create table.
            Execute.Sql(@$"
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
                CREATE UNIQUE INDEX IF NOT EXISTS {configuration.SchemaName}.ix_streams_stream_id_revision ON {configuration.TableName}(StreamId, Revision);");
        }

        public override void Down()
        {
        }
    }
}
