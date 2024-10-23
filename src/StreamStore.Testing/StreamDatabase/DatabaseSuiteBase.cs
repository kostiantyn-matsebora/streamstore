
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;



namespace StreamStore.Testing.StreamDatabase
{
    public abstract class DatabaseSuiteBase : TestSuiteBase
    {
        readonly MemoryDatabase database = new MemoryDatabase();

        public IStreamDatabase StreamDatabase => Services.GetRequiredService<IStreamDatabase>();

        public MemoryDatabase Container => database;

        protected override void RegisterServices(IServiceCollection services)
        {
            var configurator = new StreamStoreConfigurator();
            ConfigureDatabase(configurator);
            configurator.Configure(services);
        }

        protected virtual void RegisterDependencies(IServiceCollection services)
        {
        }

        protected abstract void ConfigureDatabase(IStreamStoreConfigurator configurator);

        protected override async Task SetUp()
        {
            await database.CopyTo(StreamDatabase);
        }
    }
}
