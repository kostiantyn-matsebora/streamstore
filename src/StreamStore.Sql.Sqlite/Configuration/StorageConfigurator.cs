using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Provisioning;
using StreamStore.Sql.Sqlite.Provisioning;
using StreamStore.Sql.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.Sql.Sqlite
{
    internal class StorageConfigurator: StorageConfiguratorBase
    {

        readonly SqlStorageConfiguration config = SqliteConfiguration.DefaultConfiguration;

        public StorageConfigurator()
        {
        }

        public StorageConfigurator(SqlStorageConfiguration configuration)
        {
            config = configuration.ThrowIfNull(nameof(configuration));
        }


        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
            registrator.RegisterStorage<SqlStreamStorage>();
        }

        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            // Register the PostgreSQL schema provisioner
            registrator.RegisterSchemaProvisioner<SqlSchemaProvisioner>();
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.AddSingleton(config);
            services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
            services.AddSingleton<IDapperCommandFactory, DefaultDapperCommandFactory>();
            services.AddSingleton<ISqlExceptionHandler, SqliteExceptionHandler>();
            services.AddSingleton<ISqlQueryProvider, DefaultSqlQueryProvider>();
            services.AddSingleton<IMigrator, SqliteMigrator>();
            services.AddSingleton(new MigrationConfiguration { MigrationAssembly = typeof(SqliteMigrator).Assembly });
        }
    }
}
