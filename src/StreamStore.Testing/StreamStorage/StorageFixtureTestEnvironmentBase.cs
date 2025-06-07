using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Testing.Framework;

namespace StreamStore.Testing.StreamStorage
{
    public class StorageFixtureTestEnvironmentBase: StorageTestEnvironmentBase
    {
        readonly IStorageFixture fixture;

        protected StorageFixtureTestEnvironmentBase(IStorageFixture fixture): base()
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        public override MemoryStorage Container => fixture.Container;

        protected override void ConfigureStorage(IServiceCollection services)
        {
           fixture.ConfigurePersistence(services);
        }

        protected override void SetUpInternal()
        {
        }

        protected override bool CheckIfReady()
        {
            return fixture.IsStorageReady;
        }
    }
}
