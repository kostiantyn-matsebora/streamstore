using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StreamStore.S3.Example;

namespace StreamStore.SQL.Example
{
    internal class Worker1 : WorkerBase
    {
        public Worker1(ILogger<Worker1> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
