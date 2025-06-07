using System;
using System.Threading;
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

        public MemoryStorage Container { get; }

        public bool IsStorageReady => isStorageReady;

        protected StorageFixtureBase(TStorage testStorage)
        {
            Container = CreateContainer();
            this.testStorage = testStorage.ThrowIfNull(nameof(testStorage));

            var exists = testStorage.EnsureExists();
       
            if (!exists) return;

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillStorage(provider);

            isStorageReady = true;
        }

        protected virtual MemoryStorage CreateContainer()
        {
            return new MemoryStorage();
        }

        public abstract void ConfigurePersistence(IServiceCollection services);

        ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            new StreamStoreConfigurator()
                .ConfigurePersistence(ConfigurePersistence);
            return serviceCollection.BuildServiceProvider();
        }

        static void ProvisionSchema(IServiceProvider provider)
        {
            var provisioner = provider.GetRequiredService<ISchemaProvisioner>();
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
        }

        void FillStorage(IServiceProvider provider)
        {
            var storage = provider.GetRequiredService<IStreamStorage>();
            Container.CopyTo(storage);
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
