using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.S3.B2
{
    public static class ServiceCollectionExtension
    {
        public static B2DatabaseConfigurator ConfigureB2Database(this IServiceCollection services)
        {
          return new B2DatabaseConfigurator(services);
        }

        public static IServiceCollection UseB2Database(this IServiceCollection services, ConfigurationManager configuration)
        {
          return new B2DatabaseConfigurator(services).ReadFromConfig(configuration);
        }
    }
}
