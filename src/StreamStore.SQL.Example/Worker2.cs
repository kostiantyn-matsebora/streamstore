using Microsoft.Extensions.Logging;
using StreamStore.S3.Example;

namespace StreamStore.SQL.Example
{
    internal class Worker2 : WorkerBase
    {
        public Worker2(ILogger<Worker2> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
