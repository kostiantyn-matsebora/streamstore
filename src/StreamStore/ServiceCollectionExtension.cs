using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStreamStore(this IServiceCollection services, bool compression = true)
        {
            return services
                        .AddSingleton<IStreamStore, StreamStore>()
                        .AddSingleton<ITypeRegistry>(services => TypeRegistry.CreateAndInitialize())
                        .UserNewtonsoftJsonSerializer(compression);
        }
    }
}
