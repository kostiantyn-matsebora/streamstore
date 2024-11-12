using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlDatabaseFixtureBase
    {
        public readonly MemoryDatabase Container = new MemoryDatabase();
        public readonly bool IsDatabaseReady  = false;
        protected readonly string connectionString;
        protected SqlDatabaseFixtureBase(ITestDatabase database)
        {
            var exists  = database.EnsureExists();
            this.connectionString = database.ConnectionString;

            if (!exists) return;

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillDatabase(provider);

            IsDatabaseReady = true;
        }

        public abstract void ConfigureDatabase(IStreamStoreConfigurator configurator);

        ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();

            ConfigureDatabase(configurator);
            configurator.Configure(serviceCollection);
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
