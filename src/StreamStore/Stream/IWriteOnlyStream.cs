using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace StreamStore
{
    public interface IWriteOnlyStream
    {
        Task<IWriteOnlyStream> AddAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);
        Task<Revision> SaveChangesAsync(CancellationToken cancellationToken);

        Task<IWriteOnlyStream> WriteAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);

        Task<IWriteOnlyStream> WriteAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default);
    }
}
