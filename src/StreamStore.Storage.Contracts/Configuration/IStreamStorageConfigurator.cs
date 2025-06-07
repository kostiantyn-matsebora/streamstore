using Microsoft.Extensions.DependencyInjection;

namespace StreamStore
{
    public interface IStreamStorageConfigurator
    {

        public IServiceCollection Configure();
    }
}
