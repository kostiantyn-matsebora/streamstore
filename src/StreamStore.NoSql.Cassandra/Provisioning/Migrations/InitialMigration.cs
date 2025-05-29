using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Fluent.Migrator.Core;

using StreamStore.NoSql.Cassandra.Configuration;


namespace StreamStore.NoSql.Cassandra.Provisioning.Migrations
{
    [ExcludeFromCodeCoverage]
    internal class InitialMigration : IMigrator
    {

        readonly ISession session;
        readonly CassandraStorageConfiguration config;

        public InitialMigration(ISession session, CassandraStorageConfiguration config)
        {
            this.session = session.ThrowIfNull(nameof(session));
            this.config = config.ThrowIfNull(nameof(config));
        }

        public string Name => "Initial";

        public Version Version => new Version(0, 14, 0, 0);

        public string Description => "Initial migration for Cassandra NoSQL StreamStore.";

        public async Task ApplyMigrationAsync()
        {
            await session.ExecuteAsync(new SimpleStatement(@$"CREATE TABLE IF NOT EXISTS {config.EventsTableName}
                        (id text,
                        stream_id text,
                        revision int,
                        timestamp timestamp,
                        data blob,
                        PRIMARY KEY(stream_id, revision)
                        );"));
        }
    }
}
