using System.Diagnostics.CodeAnalysis;


namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public class Worker : WorkerBase
    {
        public Worker(ILogger<Worker> logger, IStreamStore store) : base(logger, store)
        { }
    }
}

