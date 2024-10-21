using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public interface IStream : IDisposable
    {
        Task<IWriteOnlyStream> BeginWriteAsync(Revision expectedRevision, CancellationToken cancellationToken = default);

        IReadOnlyStream BeginReadAsync(Revision startFrom, CancellationToken cancellationToken = default);
    }
}
