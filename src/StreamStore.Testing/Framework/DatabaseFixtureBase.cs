using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;

namespace StreamStore.Testing.Framework
{
    public abstract class DatabaseFixtureBase<TDatabase>: IDatabaseFixture, IDisposable where TDatabase : ITestDatabase
    {
        
        readonly bool isDatabaseReady = false;
        protected readonly  TDatabase testDatabase;
        private bool disposedValue;

        public MemoryDatabase Container { get; }

        public bool IsDatabaseReady => isDatabaseReady;

        protected DatabaseFixtureBase(TDatabase testDatabase)
        {
            Container = CreateContainer();
            this.testDatabase = testDatabase.ThrowIfNull(nameof(testDatabase));

            var exists = testDatabase.EnsureExists();
       
            if (!exists) return;

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillDatabase(provider);

            isDatabaseReady = true;
        }

        protected virtual MemoryDatabase CreateContainer()
        {
            return new MemoryDatabase();
        }

        public abstract void ConfigureDatabase(ISingleTenantConfigurator configurator);

        ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();
            configurator
                .WithSingleDatabse(ConfigureDatabase)
                .Configure(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        static void ProvisionSchema(IServiceProvider provider)
        {
            var provisioner = provider.GetRequiredService<ISchemaProvisioner>();
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
        }

        void FillDatabase(IServiceProvider provider)
        {
            var database = provider.GetRequiredService<IStreamDatabase>();
            Container.CopyTo(database);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    testDatabase.Dispose();
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
