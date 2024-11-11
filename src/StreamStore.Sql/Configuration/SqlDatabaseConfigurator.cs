using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Database;
using StreamStore.Sql.Provisioning;


namespace StreamStore.Sql.Configuration
{
    public sealed class SqlDatabaseConfigurator : SqlDatabaseConfigurationBuilder
    {
        readonly IServiceCollection services;
        readonly SqlDatabaseConfiguration defaultConfig;

        public SqlDatabaseConfigurator(IServiceCollection services, SqlDatabaseConfiguration defaultConfig): base(defaultConfig)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.defaultConfig = defaultConfig.ThrowIfNull(nameof(defaultConfig));
        }

        public override IServiceCollection Configure()
        {
            var configuration = Build();
            Configure(configuration);

            return services;
        }

        public override IServiceCollection Configure(IConfiguration configuration, string sectionName)
        {
            var connectionString = configuration.GetConnectionString("StreamStore");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'StreamStore' not found in configuration");
            }

            WithConnectionString(connectionString);
            var section = configuration.GetSection(sectionName);
            if (section.Exists())
            {
                WithTable(section.GetValue("TableName", defaultConfig.TableName)!);
                WithSchema(section.GetValue("SchemaName", defaultConfig.SchemaName)!);
                ProvisionSchema(section.GetValue("ProvisionSchema", defaultConfig.ProvisionSchema));
            }

            Configure(Build());

            return services;
        }

        void Configure(SqlDatabaseConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IStreamDatabase, SqlStreamDatabase>();
            services.AddSingleton<IStreamReader, SqlStreamDatabase>();

            if (configuration.ProvisionSchema)
            {
                services.AddSingleton<SqlSchemaProvisioner>();
                services.AddHostedService<SqlSchemaProvisioningService>();
            }

        }
    }

}
