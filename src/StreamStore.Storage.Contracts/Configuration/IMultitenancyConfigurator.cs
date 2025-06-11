using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Storage
{
    public interface IMultitenancyConfigurator
    {
        void Configure(IServiceCollection services);
    }
}
