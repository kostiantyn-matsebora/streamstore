using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;


namespace StreamStore.Testing.StreamStore
{
    public abstract class StreamStoreTestEnvironmentBase : TestEnvironmentBase
    {
        readonly InMemoryStreamContainer storage = new InMemoryStreamContainer();

        public InMemoryStreamContainer Container => storage;

        public IStreamStorage Storage => Services.GetRequiredService<IStreamStorage>();

        public IStreamStore Store => Services.GetRequiredService<IStreamStore>();

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddStreamStore(ConfigureStreamStore);
        }

        protected override void Initialize()
        {
            storage.CopyToAsync(Storage).RunSynchronously();
        }

        protected abstract void ConfigureStreamStore(IStreamStoreConfigurator configurator);
    }
}
