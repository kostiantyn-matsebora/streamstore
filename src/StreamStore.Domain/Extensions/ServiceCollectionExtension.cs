using Microsoft.Extensions.DependencyInjection;
using StreamStore.Validation;

namespace StreamStore.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterDomainValidation(this IServiceCollection services)
        {
            services.AddSingleton<IStreamMutationRequestValidator, StreamMutationRequestValidator>();
            services.AddSingleton<IDuplicateEventValidator, DuplicateEventValidator>();
            services.AddSingleton<IDuplicateRevisionValidator, DuplicateRevisionValidator>();
            return services;
        }
    }
}
