using System.Diagnostics.CodeAnalysis;
using FluentMigrator;
using StreamStore.Extensions;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.PostgreSql.Provisioning.Migrations
{
    [ExcludeFromCodeCoverage]
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
                    Id text NOT NULL,
                    StreamId text NOT NULL,
                    Timestamp timestamp NOT NULL,
                    Revision integer NOT NULL,
                    Data bytea NOT NULL,
                    PRIMARY KEY (Id, StreamId)
                );

                CREATE INDEX IF NOT EXISTS ix_events_stream_id ON {configuration.TableName}(StreamId);
                CREATE INDEX IF NOT EXISTS ix_events_stream_revision ON {configuration.TableName}(Revision);
                CREATE UNIQUE INDEX IF NOT EXISTS ix_events_stream_id_revision ON {configuration.TableName}(StreamId, Revision);");
        }

        public override void Down()
        {
        }
    }
}
