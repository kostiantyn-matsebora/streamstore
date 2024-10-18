using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseSystemTextJsonSerializer(this IServiceCollection services, bool compression = true)
        {
            return services
                .AddSingleton<IEventSerializer>(services => new SystemTextJsonEventSerializer(services.GetRequiredService<ITypeRegistry>(), compression));
            
        }

        public static IServiceCollection UserNewtonsoftJsonSerializer(this IServiceCollection services, bool compression = true)
        {
            return services
                .AddSingleton<IEventSerializer>(services => new NewtonsoftEventSerializer(services.GetRequiredService<ITypeRegistry>(), compression));
        }
    }
}
