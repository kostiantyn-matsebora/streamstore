
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Testing.Framework;



namespace StreamStore.Testing.StreamStorage
{
    public abstract class StorageTestEnvironmentBase : TestEnvironmentBase
    {
        readonly InMemoryStreamContainer container = new InMemoryStreamContainer();

        public IStreamStorage StreamStorage => Services.GetRequiredService<IStreamStorage>();

        
        public virtual InMemoryStreamContainer Container => container;

        protected override sealed void RegisterServices(IServiceCollection services)
        {
           new StreamStoreConfigurator()
                .ConfigurePersistence(ConfigureStorage)
                .Configure(services);
        }

        protected abstract void ConfigureStorage(IServiceCollection services);

        protected override void Initialize()
        {
            Container.CopyToAsync(StreamStorage).GetAwaiter().GetResult();
        }
    }
}
