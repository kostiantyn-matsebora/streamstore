using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public class Reader1 : ReaderBase
    {
        public Reader1(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class Reader2 : ReaderBase
    {
        public Reader2(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class Reader3 : ReaderBase
    {
        public Reader3(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class ReaderToEnd1 : ReaderToEndBase
    {
        public ReaderToEnd1(IStreamStore store, ProgressTrackerFactory trackerFactory) : base(store, trackerFactory)
        {
        }
    }
}
