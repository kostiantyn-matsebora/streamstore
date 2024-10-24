using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Testing.Framework;


namespace StreamStore.Tests
{
    public class StreamStoreSuite : TestSuiteBase
    {
        public StreamStoreSuite()
        {
            MockDatabase = new Mock<IStreamDatabase>();
        }

        public Mock<IStreamDatabase> MockDatabase { get; }

        public IStreamStore Store => Services.GetRequiredService<IStreamStore>();

        protected override void RegisterServices(IServiceCollection services)
        {
            services.ConfigureStreamStore(configurator => 
                configurator.WithDatabase(services => services.AddSingleton<IStreamDatabase>(MockDatabase.Object)));
        }
    }
}
