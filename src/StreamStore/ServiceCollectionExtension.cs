using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStreamStore(this IServiceCollection services, bool compression = true)
        {
            services.AddSingleton<IStreamStore, StreamStore>();
            services.AddSingleton<ITypeRegistry>(services => TypeRegistry.CreateAndInitialize());
            return UserNewtonsoftJsonSerializer(services, compression);
        }

        public static IServiceCollection UseSystemTextJsonSerializer(this IServiceCollection services, bool compression = true)
        {
            services.AddSingleton<IEventSerializer>(services => new SystemTextJsonEventSerializer(services.GetRequiredService<ITypeRegistry>(),compression));
            return services;
        }

        public static IServiceCollection UserNewtonsoftJsonSerializer(this IServiceCollection services, bool compression = true)
        {
            services.AddSingleton<IEventSerializer>(services => new NewtonsoftEventSerializer(services.GetRequiredService<ITypeRegistry>(), compression));
            return services;
        }
    }
}
