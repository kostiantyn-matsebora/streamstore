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
            var created  = CreateDatabase(out databaseName);

            if (!created) return;
            

            var provider = BuildServiceProvider();

            ProvisionSchema(provider);

            FillDatabase(provider);

            IsDatabaseReady = true;
        }

        public abstract void ConfigureDatabase(IStreamStoreConfigurator configurator);

        protected abstract bool CreateDatabase(out string databaseName);

        ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();

            ConfigureDatabase(configurator);
            configurator.Configure(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        void ProvisionSchema(IServiceProvider provider)
        {
            var provisioner = provider.GetRequiredService<SqlSchemaProvisioner>();
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
        }

        void FillDatabase(IServiceProvider provider)
        {
            var database = provider.GetRequiredService<IStreamDatabase>();
            Container.CopyTo(database);
        }

    }
}
