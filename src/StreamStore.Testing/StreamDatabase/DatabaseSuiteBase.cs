
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;



namespace StreamStore.Testing.StreamDatabase
{
    public abstract class DatabaseSuiteBase : TestSuiteBase
    {
        readonly MemoryDatabase container = new MemoryDatabase();

        public IStreamDatabase StreamDatabase => Services.GetRequiredService<IStreamDatabase>();

        public virtual MemoryDatabase Container => container;

        protected override sealed void RegisterServices(IServiceCollection services)
        {
            RegisterDependencies(services);
            var configurator = new StreamStoreConfigurator();
            ConfigureDatabase(configurator);
            configurator.Configure(services);
        }

        protected virtual void RegisterDependencies(IServiceCollection services)
        {
        }

        protected abstract void ConfigureDatabase(IStreamStoreConfigurator configurator);

        protected override void SetUp()
        {
            Container.CopyTo(StreamDatabase);
        }
    }
}
