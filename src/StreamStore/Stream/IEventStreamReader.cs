﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;


namespace StreamStore
{
    public interface IEventStreamReader: IAsyncEnumerable<EventEntity>
    {
        Task<EventEntityCollection> ReadToEndAsync(CancellationToken cancellationToken = default);
    }
}