using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;

namespace StreamStore.Testing.Framework
{
    public abstract class DatabaseFixtureBase<TDatabase> where TDatabase : ITestDatabase
    {
        public readonly MemoryDatabase Container = new MemoryDatabase();
        public readonly bool IsDatabaseReady = false;
        protected readonly  TDatabase testDatabase;

        protected DatabaseFixtureBase(TDatabase testDatabase)
        {
            this.testDatabase = testDatabase.ThrowIfNull(nameof(testDatabase));

            var exists = testDatabase.EnsureExists();
       
            if (!exists) return;

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillDatabase(provider);

            IsDatabaseReady = true;
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
    }
}
