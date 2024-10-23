using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;


namespace StreamStore.Testing.StreamStore
{
    public abstract class StreamStoreSuiteBase : TestSuiteBase
    {
        readonly MemoryDatabase database = new MemoryDatabase();


        public MemoryDatabase Container => database;

        public IStreamStore Store => Services.GetRequiredService<IStreamStore>();

        protected override void RegisterServices(IServiceCollection services)
        {
            services.ConfigureStreamStore(ConfigureStreamStore);
        }

        protected override async Task SetUp()
        {
            await database.CopyTo(Services.GetRequiredService<IStreamDatabase>());
        }

        protected abstract void ConfigureStreamStore(IStreamStoreConfigurator configurator);

    }
}
