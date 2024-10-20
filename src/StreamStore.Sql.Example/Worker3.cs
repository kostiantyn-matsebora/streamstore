using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StreamStore.S3.Example;

namespace StreamStore.SQL.Example
{
    internal class Worker3 : WorkerBase
    {
        public Worker3(ILogger<Worker3> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
