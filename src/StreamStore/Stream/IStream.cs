using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public interface IStream
    {
        Task<IWriteOnlyStream> BeginWriteAsync(Revision expectedRevision, CancellationToken cancellationToken = default);

        IReadOnlyStream BeginRead(Revision startFrom, CancellationToken cancellationToken = default);
    }
}
