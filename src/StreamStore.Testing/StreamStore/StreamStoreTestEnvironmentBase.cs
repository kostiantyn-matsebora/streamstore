using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;


namespace StreamStore.Testing.StreamStore
{
    public abstract class StreamStoreTestEnvironmentBase : TestEnvironmentBase
    {
        readonly MemoryDatabase database = new MemoryDatabase();

        public MemoryDatabase Container => database;

        public IStreamStorage Database => Services.GetRequiredService<IStreamStorage>();

        public IStreamStore Store => Services.GetRequiredService<IStreamStore>();

        protected override void RegisterServices(IServiceCollection services)
        {
            services.ConfigureStreamStore(ConfigureStreamStore);
        }

        protected override void SetUpInternal()
        {
            database.CopyTo(Database);
        }

        protected abstract void ConfigureStreamStore(IStreamStoreConfigurator configurator);
    }
}
