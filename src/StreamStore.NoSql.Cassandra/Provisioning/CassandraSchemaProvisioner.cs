using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Cassandra.Fluent.Migrator;
using Cassandra.Fluent.Migrator.Core;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Provisioning.Migrations;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra.Provisioning
{
    [ExcludeFromCodeCoverage]
    internal class CassandraSchemaProvisioner : ISchemaProvisioner
    {
        readonly ICassandraSessionFactory sessionFactory;
        private readonly CassandraStorageConfiguration config;

        public CassandraSchemaProvisioner(ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.config = config.ThrowIfNull(nameof(config));
        }

        public Task ProvisionSchemaAsync(CancellationToken token)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(sessionFactory.Open())
                .AddSingleton(config)
                .AddCassandraFluentMigratorServices()
                .AddSingleton<IMigrator, InitialMigration>()
                .AddSingleton<IMigrator, AddCustomProperties>()
                .BuildServiceProvider();
            
            var migrator = serviceProvider.GetRequiredService<ICassandraMigrator>();
            migrator.Migrate();
            return Task.CompletedTask;
        }
    }
}
