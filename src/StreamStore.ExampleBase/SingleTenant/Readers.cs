using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public class Reader1 : ReaderBase
    {
        public Reader1(ILogger<Reader1> logger, IStreamStore store) : base(logger, store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class Reader2 : ReaderBase
    {
        public Reader2(ILogger<Reader2> logger, IStreamStore store) : base(logger, store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class Reader3 : ReaderBase
    {
        public Reader3(ILogger<Reader3> logger, IStreamStore store) : base(logger, store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public class ReaderToEnd1 : ReaderToEndBase
    {
        public ReaderToEnd1(ILogger<ReaderToEnd1> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
