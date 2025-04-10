
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Testing.Framework;



namespace StreamStore.Testing.StreamStorage
{
    public abstract class StorageTestEnvironmentBase : TestEnvironmentBase
    {
        readonly MemoryStorage container = new MemoryStorage();

        public IStreamStorage StreamStorage => Services.GetRequiredService<IStreamStorage>();

        public virtual MemoryStorage Container => container;

        protected override sealed void RegisterServices(IServiceCollection services)
        {
            ConfiguratorFactory.StoreConfigurator
                .WithSingleStorage(ConfigureStorage)
                .Configure(services);
        }

        protected abstract void ConfigureStorage(ISingleTenantConfigurator configurator);

        protected override void SetUpInternal()
        {
            Container.CopyTo(StreamStorage);
        }
    }
}
