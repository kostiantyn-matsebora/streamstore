using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;


namespace StreamStore.ExampleBase.Services.SingleTenant
{
    [ExcludeFromCodeCoverage]
    internal class Reader1 : ReaderServiceBase
    {
        public Reader1(IStreamStore store, WorkerRegistry trackerFactory) : base(store, trackerFactory, 1)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Reader2 : ReaderServiceBase
    {
        public Reader2(IStreamStore store, WorkerRegistry trackerFactory) : base(store, trackerFactory, 2)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Reader3 : ReaderServiceBase
    {
        public Reader3(IStreamStore store, WorkerRegistry workerRegistry) : base(store, workerRegistry, 3)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class ReaderToEnd1 : ReaderToEndServiceBase
    {
        public ReaderToEnd1(IStreamStore store, WorkerRegistry workerRegistry) : base(store, workerRegistry, 1)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Writer1 : WriterServiceBase
    {
        public Writer1(IStreamStore store, WorkerRegistry workerRegistry) : base(store, workerRegistry, 1)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Writer2 : WriterServiceBase
    {
        public Writer2(IStreamStore store, WorkerRegistry workerRegistry) : base(store, workerRegistry, 2)
        {
        }
    }
    [ExcludeFromCodeCoverage]
    internal class Writer3 : WriterServiceBase
    {
        public Writer3(IStreamStore store, WorkerRegistry workerRegisty) : base(store, workerRegisty, 3)
        {
        }
    }
}
