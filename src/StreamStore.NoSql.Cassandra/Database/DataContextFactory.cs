using Cassandra;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class DataContextFactory
    {
        readonly TypeMapFactory mapFactory;

        public DataContextFactory(TypeMapFactory mapFactory)
        {
          this.mapFactory = mapFactory.ThrowIfNull(nameof(mapFactory));
        }

        public DataContext Create(ISession session)
        {
            return new DataContext(mapFactory, session);
        }
    }
}
