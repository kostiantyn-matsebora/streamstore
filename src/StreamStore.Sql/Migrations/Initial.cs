using FluentMigrator;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Migrations
{
    [Migration(1)]
    public class Initial : Migration
    {
        private SqlStorageConfiguration configuration;

        public Initial(SqlStorageConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }
       

        public override void Up()
        {
            Create
                .Table(configuration.TableName)
                .InSchema(configuration.SchemaName)
                .WithColumn("Id").AsString().NotNullable()
                .WithColumn("StreamId").AsString().NotNullable().Indexed($"{configuration.SchemaName}.ix_streams_stream_id")
                .WithColumn("Timestamp").AsDateTime2().NotNullable()
                .WithColumn("Revision").AsInt32().NotNullable().Indexed($"{configuration.SchemaName}.ix_streams_stream_revision")
                .WithColumn("Data").AsBinary().NotNullable();


             Create.PrimaryKey().OnTable(configuration.TableName).WithSchema(configuration.SchemaName).Columns("Id", "StreamId");
             Create
                .UniqueConstraint($"{configuration.SchemaName}.ix_streams_stream_id_revision")
                .OnTable(configuration.TableName)
                .WithSchema(configuration.SchemaName)
                .Columns("StreamId", "Revision");
        }

        public override void Down()
        {
            Delete.Table(configuration.FullTableName);
        }
    }
}
