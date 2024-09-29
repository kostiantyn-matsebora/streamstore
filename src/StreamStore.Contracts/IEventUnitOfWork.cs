using System.Threading.Tasks;
using System.Threading;
using System;

namespace StreamStore
{
    public interface IEventUnitOfWork: IDisposable
    {
        public Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        IEventUnitOfWork Add(Id eventId, int revision, DateTime timestamp, string data);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
