using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors.Postgres;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Provisioning;


namespace StreamStore.Sql.PostgreSql.Provisioning
{
    internal class PostgreSqlMigrator: IMigrator
    {
        readonly SqlStorageConfiguration storageConfig;
        readonly MigrationConfiguration migrationConfig;

        public PostgreSqlMigrator(SqlStorageConfiguration storageConfig, MigrationConfiguration migrationConfig)
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
                        .AddPostgres92()
                        .WithGlobalConnectionString(storageConfig.ConnectionString)
                        .ScanIn(migrationConfig.MigrationAssembly).For.All())
                    .AddSingleton(new PostgresOptions { ForceQuote = false })
                    .AddLogging(lb => lb.AddFluentMigratorConsole())
                    .BuildServiceProvider(false);

            serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();


        }
    }
}
