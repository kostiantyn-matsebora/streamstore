using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using StreamStore.S3.Example;

namespace StreamStore.SQL.Example
{
    [ExcludeFromCodeCoverage]
    internal class Worker3 : WorkerBase
    {
        public Worker3(ILogger<Worker3> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
