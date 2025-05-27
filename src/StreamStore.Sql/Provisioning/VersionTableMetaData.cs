using FluentMigrator.Runner.VersionTableInfo;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Provisioning
{
    internal sealed class VersionTableMetaData: DefaultVersionTableMetaData
    {
        readonly SqlStorageConfiguration configuration;

#pragma warning disable CS0618 // Type or member is obsolete
        public VersionTableMetaData(SqlStorageConfiguration configuration)
        {
           this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }
#pragma warning restore CS0618 // Type or member is obsolete
        public override string TableName => "StreamStoreVersionInfo";

        public override string SchemaName => configuration.SchemaName;
    }
}
