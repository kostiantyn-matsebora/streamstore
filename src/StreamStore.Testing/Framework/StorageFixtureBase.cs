using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Extensions;
using StreamStore.Provisioning;

namespace StreamStore.Testing.Framework
{
    public abstract class StorageFixtureBase<TStorage>: IStorageFixture, IDisposable where TStorage : ITestStorage
    {
        
        readonly bool isStorageReady = false;
        protected readonly  TStorage testStorage;
        private bool disposedValue;

        public InMemoryStreamContainer Container { get; }

        public bool IsStorageReady => isStorageReady;

        protected StorageFixtureBase(TStorage testStorage)
        {
            this.testStorage = testStorage.ThrowIfNull(nameof(testStorage));
            
            Container = CreateInMemoryContainer();

            if (this.testStorage.EnsureExists()) return;

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillStorage(provider);

            isStorageReady = true;
        }

        protected virtual InMemoryStreamContainer CreateInMemoryContainer()
        {
            return new InMemoryStreamContainer();
        }

        public abstract void ConfigurePersistence(IServiceCollection services);

        ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = 
                new StreamStoreConfigurator()
                .ConfigurePersistence(ConfigurePersistence)
                .Configure(new ServiceCollection());

            return serviceCollection.BuildServiceProvider();
        }

        static void ProvisionSchema(IServiceProvider provider)
        {
            var provisioner = provider.GetRequiredService<ISchemaProvisioner>();
            provisioner.ProvisionSchemaAsync(CancellationToken.None).RunSynchronously();
        }

        void FillStorage(IServiceProvider provider)
        {
            var storage = provider.GetRequiredService<IStreamStorage>();
            Container.CopyToAsync(storage).RunSynchronously();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    testStorage.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
