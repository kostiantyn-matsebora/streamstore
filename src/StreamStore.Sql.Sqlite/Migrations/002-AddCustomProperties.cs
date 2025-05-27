using FluentMigrator;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Sqlite.Migrations
{
    [Migration(2, "Add custom properties column to the event table")]
    public class AddCustomProperties : Migration
    {
        readonly SqlStorageConfiguration config;

        public AddCustomProperties(SqlStorageConfiguration config)
        {
          this.config = config.ThrowIfNull(nameof(config));
        }

        public override void Down()
        {

        }

        public override void Up()
        {
           Create
                .Column("CustomProperties")
                .OnTable(config.TableName)
                .InSchema(config.SchemaName)
                .AsString()
                .Nullable()
                .WithColumnDescription("Custom properties for the event, serialized as JSON.");
        }
    }
}
