using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql;


namespace StreamStore.SQL.Sqlite
{
    public sealed class SqlDatabaseConfigurator : SqlDatabaseConfigurationBuilder
    {
        readonly IServiceCollection services;
        public SqlDatabaseConfigurator(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public override IServiceCollection Configure()
        {
            var configuration = Build();
            Configure(configuration);

            return services;
        }

        public override IServiceCollection Configure(IConfiguration configuration, string sectionName = "StreamStore:Sql")
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
                WithTable(section.GetValue("TableName", "Events")!);
                WithSchema(section.GetValue("SchemaName", "main")!);
                ProvisionSchema(section.GetValue("ProvisionSchema", true));
            }

            Configure(Build());

            return services;
        }

        void Configure(SqlDatabaseConfiguration configuration)
        {
            //services.AddSingleton<IDbConnectionFactory, SqliteDapperConnectionFactory>();

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
