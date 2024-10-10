using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.InMemory
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryStreamDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IStreamDatabase, InMemoryStreamDatabase>();
            return services;
        }
    }
}
