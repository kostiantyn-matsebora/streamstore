using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql.Provisioning;
using StreamStore.Sql.Provisioning;
using StreamStore.Sql.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        SqlStorageConfiguration config = PostgresConfiguration.DefaultConfiguration;

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
            services.AddSingleton<IDbConnectionFactory, PostgresConnectionFactory>();
            services.AddSingleton<IDapperCommandFactory, DefaultDapperCommandFactory>();
            services.AddSingleton<ISqlExceptionHandler, PostgresExceptionHandler>();
            services.AddSingleton<ISqlQueryProvider, DefaultSqlQueryProvider>();
            services.AddSingleton<IMigrator, PostgreSqlMigrator>();
            services.AddSingleton(new MigrationConfiguration { MigrationAssembly = typeof(PostgreSqlMigrator).Assembly });
        }
    }
}