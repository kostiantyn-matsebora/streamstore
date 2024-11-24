using System;
using Cassandra;
using Cassandra.Mapping;


namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraMapper: IMapper, IDisposable
    {
    }

    internal sealed class CassandraMapperDecorator: Mapper, ICassandraMapper
    {
        readonly ISession session;
        private bool disposedValue;

        public CassandraMapperDecorator(ISession session, MappingConfiguration mapping) : base(session, mapping)
        {
            this.session = session.ThrowIfNull(nameof(session));
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    session.Dispose();
                }
                disposedValue = true;
            }
        }

    }
}
