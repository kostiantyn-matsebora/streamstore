
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;


namespace StreamStore.Configuration.Storage
{
    class ConfiguratorBase
    {
        protected readonly ServiceCollection services = new ServiceCollection();

        protected ConfiguratorBase()
        {
            // Register default validators
            services.RegisterDomainValidation();
        }
    }
}
