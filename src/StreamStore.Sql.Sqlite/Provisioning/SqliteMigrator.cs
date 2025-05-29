using System.Diagnostics.CodeAnalysis;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Provisioning;


namespace StreamStore.Sql.Sqlite.Provisioning
{
    [ExcludeFromCodeCoverage]
    internal class SqliteMigrator: IMigrator
    {
        readonly SqlStorageConfiguration storageConfig;
        readonly MigrationConfiguration migrationConfig;

        public SqliteMigrator(SqlStorageConfiguration storageConfig, MigrationConfiguration migrationConfig)
        {
           this.storageConfig = storageConfig.ThrowIfNull(nameof(storageConfig));
           this.migrationConfig = migrationConfig.ThrowIfNull(nameof(migrationConfig));
        }

        public void Migrate()
        {
            var serviceProvider = new ServiceCollection()
                    .AddSingleton(storageConfig)
                    .AddSingleton<IVersionTableMetaData, VersionTableMetaData>()
                    .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddSQLite()
                        .WithGlobalConnectionString(storageConfig.ConnectionString)
                        .ScanIn(migrationConfig.MigrationAssembly).For.All())
                    .AddLogging(lb => lb.AddFluentMigratorConsole())
                    .BuildServiceProvider(false);

            serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
        }
    }
}
