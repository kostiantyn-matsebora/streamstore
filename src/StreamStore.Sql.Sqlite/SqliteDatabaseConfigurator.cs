using System;
using Dapper.Extensions;
using Dapper.Extensions.MiniProfiler;
using Dapper.Extensions.SQLite;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.SQL.Sqlite
{
    public sealed class SqliteDatabaseConfigurator: SqliteDatabaseConfigurationBuilder
    {
        readonly IServiceCollection services;
        public SqliteDatabaseConfigurator(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public override IServiceCollection Configure()
        {
            var configuration = Build();
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

            return services;
        }

    }

    public interface IConfigurator
    {
        IServiceCollection Configure();
    }
}
