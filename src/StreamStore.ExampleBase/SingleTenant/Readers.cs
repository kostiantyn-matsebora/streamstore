using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public class Reader1 : ReaderBase
    {
        public Reader1(ILoggerFactory loggerFactory, IStreamStore store) : base(loggerFactory.CreateLogger(nameof(Reader1)), store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class Reader2 : ReaderBase
    {
        public Reader2(ILoggerFactory loggerFactory, IStreamStore store) : base(loggerFactory.CreateLogger(nameof(Reader2)), store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class Reader3 : ReaderBase
    {
        public Reader3(ILoggerFactory loggerFactory, IStreamStore store) : base(loggerFactory.CreateLogger(nameof(Reader3)), store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class ReaderToEnd1 : ReaderToEndBase
    {
        public ReaderToEnd1(ILoggerFactory loggerFactory, IStreamStore store) : base(loggerFactory.CreateLogger(nameof(ReaderToEnd1)), store)
        {
        }
    }
}
