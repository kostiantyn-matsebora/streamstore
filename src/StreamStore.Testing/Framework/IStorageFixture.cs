using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Testing.Framework
{
    public interface IStorageFixture
    {
        MemoryStorage Container { get; }
        void ConfigurePersistence(IServiceCollection services);

        bool IsStorageReady { get; }
    }
}
