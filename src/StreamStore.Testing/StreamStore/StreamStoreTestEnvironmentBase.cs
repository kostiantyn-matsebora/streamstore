using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;


namespace StreamStore.Testing.StreamStore
{
    public abstract class StreamStoreTestEnvironmentBase : TestEnvironmentBase
    {
        readonly MemoryStorage storage = new MemoryStorage();

        public MemoryStorage Container => storage;

        public IStreamStorage Storage => Services.GetRequiredService<IStreamStorage>();

        public IStreamStore Store => Services.GetRequiredService<IStreamStore>();

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddStreamStore(ConfigureStreamStore);
        }

        protected override async Task SetUpAsync()
        {
            await storage.CopyToAsync(Storage);
        }

        protected abstract void ConfigureStreamStore(IStreamStoreConfigurator configurator);
    }
}
