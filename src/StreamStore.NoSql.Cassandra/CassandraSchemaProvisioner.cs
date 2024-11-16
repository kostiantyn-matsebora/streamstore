using System.Threading;
using System.Threading.Tasks;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Provisioning;


namespace StreamStore.NoSql.Cassandra
{
    internal class CassandraSchemaProvisioner: ISchemaProvisioner
    {
        readonly CassandraSessionFactory sessionFactory;
        private readonly DataContextFactory dataContextFactory;

        public CassandraSchemaProvisioner(CassandraSessionFactory sessionFactory, DataContextFactory dataContextFactory)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.dataContextFactory = dataContextFactory.ThrowIfNull(nameof(dataContextFactory));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
           using (var session = sessionFactory.CreateSession())
            {
                var ctx = dataContextFactory.Create(session);
                await ctx.EventPerStream.CreateIfNotExistsAsync();
                await ctx.RevisionPerStream.CreateIfNotExistsAsync();
                await ctx.Events.CreateIfNotExistsAsync();
            }
        }
    }
}
