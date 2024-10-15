﻿using System.Diagnostics.CodeAnalysis;

namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public  class Worker3: Worker
    {
        public Worker3(ILogger<Worker3> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
