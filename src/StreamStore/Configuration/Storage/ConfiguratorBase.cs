
using Microsoft.Extensions.DependencyInjection;


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
