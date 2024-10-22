using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IStreamStoreConfigurator UseSystemTextJsonSerializer(this IStreamStoreConfigurator configurator, bool compression = true)
        {
            configurator.WithEventSerializer<SystemTextJsonEventSerializer>();
            return configurator;
        }

        public static IStreamStoreConfigurator UserNewtonsoftJsonSerializer(this IStreamStoreConfigurator configurator, bool compression = true)
        {
            configurator.WithEventSerializer<NewtonsoftEventSerializer>();
            return configurator;
        }
    }
}
