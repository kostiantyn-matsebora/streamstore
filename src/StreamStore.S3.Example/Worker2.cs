using System.Diagnostics.CodeAnalysis;

namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public class Worker2 : WorkerBase
    {
        public Worker2(ILogger<Worker2> logger, IStreamStore store) : base(logger, store)
        { }
    }
}
