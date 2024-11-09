using System.Data.Entity;
using StreamStore.SQL;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlDatabaseFixtureBase : IDisposable
    {
        private bool disposed = false;

        public readonly MemoryDatabase Container = new MemoryDatabase();

        protected SqlDatabaseFixtureBase()
        {

            ProvisionDatabase();
        }


        void ProvisionDatabase()
        {
            CreateDatabase();
            var connectionFactory = CreateConnectionFactory();
            var commandFactory = CreateCommandFactory();
            var exceptionHandler = CreateExceptionHandler();

            var provisioner = new SqlSchemaProvisioner(connectionFactory, commandFactory);
            provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();

            var database = new SqlStreamDatabase(connectionFactory, commandFactory, exceptionHandler);
            Container.CopyTo(database);
        }

        protected abstract void CreateDatabase();

        protected abstract IDbConnectionFactory CreateConnectionFactory();
        protected abstract IDapperCommandFactory CreateCommandFactory();
        protected abstract ISqlExceptionHandler CreateExceptionHandler();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
