using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    internal class Writer1 : WriterBase
    {
        public Writer1(ILoggerFactory loggerFactory, IStreamStore store) : base(loggerFactory.CreateLogger(nameof(Writer1)), store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Writer2 : WriterBase
    {
        public Writer2(ILoggerFactory loggerFactory, IStreamStore store) : base(loggerFactory.CreateLogger(nameof(Writer2)), store)
        {
        }
    }
    [ExcludeFromCodeCoverage]
    internal class Writer3 : WriterBase
    {
        public Writer3(ILoggerFactory loggerFactory,  IStreamStore store) : base(loggerFactory.CreateLogger(nameof(Writer3)), store)
        {
        }
    }
}
