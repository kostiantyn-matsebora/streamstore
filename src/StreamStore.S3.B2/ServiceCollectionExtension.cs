using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage;

namespace StreamStore.S3.B2
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddB2(this IServiceCollection services)
        {
            return services.ConfigurePersistence(new StorageConfigurator());
        }
    }
}
