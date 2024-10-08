using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.InMemory
{
    public static class ServiceColelctionExtension
    {
        public static IServiceCollection AddInMemoryStreamDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IStreamDatabase, InMemoryStreamDatabase>();
            return services;
        }
    }
}
