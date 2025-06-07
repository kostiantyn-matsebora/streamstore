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

        public static IServiceCollection CopyFrom(this IServiceCollection target, IServiceCollection source)
        {
            source.ThrowIfNull(nameof(source));
            target.ThrowIfNull(nameof(target));
            foreach (var service in source)
            {
                target.Add(service);
            }
            return target;
        }
    }
}
