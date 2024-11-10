using StreamStore.Sql.API;
using StreamStore.Sql.Database;
using StreamStore.Sql.Provisioning;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlDatabaseFixtureBase
    {
        public readonly MemoryDatabase Container = new MemoryDatabase();
        
        public string DatabaseName { get; }

        protected SqlDatabaseFixtureBase()
        {
            DatabaseName = GenerateDatabaseName();
            ProvisionDatabase();
        }


        void ProvisionDatabase()
        {
            CreateDatabase();

            ProvisionSchema();

            FillSchema();
        }

        
        protected abstract string GenerateDatabaseName();
        protected abstract void CreateDatabase();
        protected abstract IDbConnectionFactory CreateConnectionFactory();
        protected abstract IDapperCommandFactory CreateCommandFactory();
        protected abstract ISqlExceptionHandler CreateExceptionHandler();
        protected abstract ISqlProvisionQueryProvider CreateProvisionQueryProvider();

        void FillSchema()
        {
            var database = new SqlStreamDatabase(CreateConnectionFactory(), CreateCommandFactory(), CreateExceptionHandler());
            Container.CopyTo(database);
        }

        void ProvisionSchema()
        {
            var provisioner = new SqlSchemaProvisioner(CreateConnectionFactory(), CreateProvisionQueryProvider());
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
        }
    }
}
