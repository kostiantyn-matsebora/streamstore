using System;
using Dapper.Extensions;
using Dapper.Extensions.MiniProfiler;
using Dapper.Extensions.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.SQL.Sqlite
{
    public sealed class SqliteDatabaseConfigurator : SqliteDatabaseConfigurationBuilder
    {
        readonly IServiceCollection services;
        public SqliteDatabaseConfigurator(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public override IServiceCollection Configure()
        {
            var configuration = Build();
            Configure(configuration);

            return services;
        }

     

        public override IServiceCollection Configure(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StreamStore");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'StreamStore' not found in configuration");
            }
            
            WithConnectionString(connectionString);
            var section = configuration.GetSection("StreamStore:Sqlite");
            if (section.Exists())
            {
                WithTable(section.GetValue("TableName", "Events")!);
                WithSchema(section.GetValue("SchemaName", "main")!);
                ProvisionSchema(section.GetValue("ProvisionSchema", true));
                if (section.GetValue("ProfilingEnabled", false))
                {
                    EnableProfiling();
                }
            }

            Configure(Build());

            return services;
        }

        void Configure(SqliteDatabaseConfiguration configuration)
        {
            services.AddDapperForSQLite();
            services.AddDapperConnectionStringProvider<SqliteDapperConnectionStringProvider>();

            services.AddSingleton(configuration);
            services.AddSingleton<IStreamDatabase, SqliteStreamDatabase>();

            if (configuration.ProvisionSchema)
            {
                services.AddSingleton<SqliteSchemaProvisioner>();
                services.AddHostedService<SqliteSchemaProvisioningService>();
            }

            if (configuration.EnableProfiling)
            {
                services.AddMiniProfilerForDapper();
            }
        }
    }

    public interface IConfigurator
    {
        IServiceCollection Configure();
        IServiceCollection Configure(IConfiguration configuration);

    }
}
