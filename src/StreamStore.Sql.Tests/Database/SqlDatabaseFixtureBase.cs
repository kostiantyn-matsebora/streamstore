using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Provisioning;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlDatabaseFixtureBase
    {
        public readonly MemoryDatabase Container = new MemoryDatabase();
        public readonly bool IsDatabaseReady  = false;

        protected readonly string databaseName;

        protected SqlDatabaseFixtureBase()
        {
            databaseName = CreateDatabase();

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillSchema(provider);

            IsDatabaseReady = true;
        }

        public abstract void ConfigureDatabase(IStreamStoreConfigurator configurator);

        protected abstract string CreateDatabase();

        ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();

            ConfigureDatabase(configurator);
            configurator.Configure(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        void FillSchema(IServiceProvider provider)
        {
            var database = provider.GetRequiredService<IStreamDatabase>();
            Container.CopyTo(database);
        }

        void ProvisionSchema(IServiceProvider provider)
        {
            var provisioner = provider.GetRequiredService<SqlSchemaProvisioner>();
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
        }
    }
}
