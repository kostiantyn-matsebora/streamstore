using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    internal class Writer1 : WriterBase
    {
        public Writer1(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Writer2 : WriterBase
    {
        public Writer2(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }
    [ExcludeFromCodeCoverage]
    internal class Writer3 : WriterBase
    {
        public Writer3(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }
}
