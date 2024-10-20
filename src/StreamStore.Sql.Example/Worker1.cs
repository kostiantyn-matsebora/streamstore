using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using StreamStore.S3.Example;

namespace StreamStore.SQL.Example
{
    [ExcludeFromCodeCoverage]
    internal class Worker1 : WorkerBase
    {
        public Worker1(ILogger<Worker1> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
