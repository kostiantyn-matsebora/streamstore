using System.Diagnostics.CodeAnalysis;
using FluentMigrator;
using StreamStore.Extensions;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql.Provisioning.Migrations
{
    [ExcludeFromCodeCoverage]
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
                 .AsString()
                 .Nullable()
                 .WithColumnDescription("Custom properties for the event, serialized as JSON.");
        }
    }
}
