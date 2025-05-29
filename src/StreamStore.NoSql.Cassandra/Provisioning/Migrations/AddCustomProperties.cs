using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Fluent.Migrator.Core;
using StreamStore.NoSql.Cassandra.Configuration;


namespace StreamStore.NoSql.Cassandra.Provisioning.Migrations
{
    [ExcludeFromCodeCoverage]
    internal class AddCustomProperties: IMigrator
    {
        
        readonly ISession session;
        readonly CassandraStorageConfiguration config;

        public AddCustomProperties(ISession session, CassandraStorageConfiguration config)
        {
           this.session = session.ThrowIfNull(nameof(session));
           this.config = config.ThrowIfNull(nameof(config));
        }

        public string Name => "AddCustomProperties";

        public Version Version => new Version(0, 7, 0, 0);

        public string Description => "Add CustomProperties column to the Events table in Cassandra NoSQL StreamStore.";

        public async Task ApplyMigrationAsync()
        {
            await session.ExecuteAsync(new SimpleStatement(@$"ALTER TABLE {config.EventsTableName} ADD custom_properties map<text,text>;"));
        }
    }
}
