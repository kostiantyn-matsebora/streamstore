
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.SQL.Sqlite
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection UseSqliteStreamDatabase(this IServiceCollection services, string connectionString, bool provisionSchema = true)
        {

            var configurator = new SqliteDatabaseConfigurator(services);
            configurator
                .WithConnectionString(connectionString)
                .WithSchemaProvisioning(provisionSchema);
            configurator.Configure();
            return services;
        }

        public static SqliteDatabaseConfigurator ConfigureSqliteStreamDatabase(this IServiceCollection services)
        {
            return new SqliteDatabaseConfigurator(services);
        }
    }
}
