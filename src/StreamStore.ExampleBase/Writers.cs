using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using StreamStore.S3.Example;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class Writer1 : WriterBase
    {
        public Writer1(ILogger<Writer1> logger, IStreamStore store) : base(logger, store)
        {
        }
    }

    [ExcludeFromCodeCoverage]
    internal class Writer2 : WriterBase
    {
        public Writer2(ILogger<Writer2> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
    [ExcludeFromCodeCoverage]
    internal class Writer3 : WriterBase
    {
        public Writer3(ILogger<Writer3> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
