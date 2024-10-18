using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Serialization.Protobuf
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseProtobufSerializer(this IServiceCollection services, bool compression = true)
        {
            return services
                .AddSingleton<IEventSerializer>(services => new ProtobufEventSerializer(services.GetRequiredService<ITypeRegistry>(), compression));
        }
    }
}
