using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Storage
{
    public interface IStorageConfigurator
    {
        void Configure(IServiceCollection services);
    }
}
