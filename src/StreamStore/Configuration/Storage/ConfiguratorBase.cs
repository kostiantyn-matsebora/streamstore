
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage.Validation;
using StreamStore.Validation;

namespace StreamStore.Configuration.Storage
{
    class ConfiguratorBase
    {
        protected readonly ServiceCollection services = new ServiceCollection();

        protected ConfiguratorBase()
        {
            // Register default validators
            services.AddSingleton<IStreamMutationRequestValidator, StreamMutationRequestValidator>();
            services.AddSingleton<IDuplicateEventValidator, DuplicateEventValidator>();
            services.AddSingleton<IDuplicateRevisionValidator, DuplicateRevisionValidator>();
        }
    }
}
