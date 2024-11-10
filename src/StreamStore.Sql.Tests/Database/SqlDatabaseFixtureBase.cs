using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;
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
            ProvisionDatabase();
            IsDatabaseReady = true;
        }


        void ProvisionDatabase()
        {
            ProvisionSchema();

            FillSchema();
        }

        public abstract string GetConnectionString();
        protected abstract string CreateDatabase();
        protected abstract IDbConnectionFactory CreateConnectionFactory();
        protected abstract ISqlExceptionHandler CreateExceptionHandler();
        protected abstract ISqlProvisioningQueryProvider CreateProvisionQueryProvider();

        protected SqlDatabaseConfiguration CreateConfiguration()
        {
            return new SqlDatabaseConfigurationBuilder()
                        .WithConnectionString(GetConnectionString())
                        .Build();
        }

        void FillSchema()
        {
            var database = new SqlStreamDatabase(
                                connectionFactory: CreateConnectionFactory(),
                                commandFactory: new DefaultDapperCommandFactory(new DefaultSqlQueryProvider(CreateConfiguration())),
                                exceptionHandler: CreateExceptionHandler());

            Container.CopyTo(database);
        }

        void ProvisionSchema()
        {
            var provisioner = new SqlSchemaProvisioner(CreateConnectionFactory(), CreateProvisionQueryProvider());
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
        }
    }
}
