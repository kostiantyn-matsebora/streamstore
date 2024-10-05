﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3Client : IDisposable
    {
        Task<byte[]?> FindBlobAsync(string key, CancellationToken token);
        Task<IS3ReadonlyMetadataCollection> FindBlobMetadataAsync(string key, CancellationToken token);
        Task UploadBlobAsync(string key, byte[] data, IS3ReadonlyMetadataCollection metadata, CancellationToken token);
        Task DeleteBlobAsync(string key, CancellationToken token);
    }
}
