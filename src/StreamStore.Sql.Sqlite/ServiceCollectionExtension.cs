using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.SQL.Sqlite
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseSqliteStreamDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            var configurator = new SqliteDatabaseConfigurator(services);
            configurator.Configure(configuration);
            return services;
        }

        public static SqliteDatabaseConfigurator ConfigureSqliteStreamDatabase(this IServiceCollection services)
        {
            return new SqliteDatabaseConfigurator(services);
        }
    }
}
