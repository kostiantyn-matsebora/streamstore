using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra
{
    internal class CassandraSchemaProvisioner: ISchemaProvisioner
    {
        readonly ICassandraSessionFactory sessionFactory;
        private readonly DataContextFactory dataContextFactory;

        public CassandraSchemaProvisioner(ICassandraSessionFactory sessionFactory, DataContextFactory dataContextFactory)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.dataContextFactory = dataContextFactory.ThrowIfNull(nameof(dataContextFactory));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
           using (var session = sessionFactory.Open())
            {
                var ctx = dataContextFactory.Create(session);
                await ctx.Events.CreateIfNotExistsAsync();
            }
        }
    }
}
