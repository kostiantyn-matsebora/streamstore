
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;



namespace StreamStore.Testing.StreamDatabase
{
    public abstract class DatabaseTestEnvironmentBase : TestEnvironmentBase
    {
        readonly MemoryDatabase container = new MemoryDatabase();

        public IStreamStorage StreamDatabase => Services.GetRequiredService<IStreamStorage>();

        public virtual MemoryDatabase Container => container;

        protected override sealed void RegisterServices(IServiceCollection services)
        {
            new StreamStoreConfigurator()
                .WithSingleDatabase(ConfigureDatabase)
                .Configure(services);
        }

        protected abstract void ConfigureDatabase(ISingleTenantConfigurator configurator);

        protected override void SetUpInternal()
        {
            Container.CopyTo(StreamDatabase);
        }
    }
}
