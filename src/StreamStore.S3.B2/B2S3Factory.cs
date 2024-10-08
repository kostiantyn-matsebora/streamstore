﻿using System;

using Bytewizer.Backblaze.Client;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.B2
{
    internal class B2S3Factory : IS3Factory
    {
        readonly B2StreamDatabaseSettings settings;
        readonly BackblazeClient? client; //TODO: create pool of clients
        readonly S3InMemoryStreamLockStorage storage = new S3InMemoryStreamLockStorage();

        public B2S3Factory(B2StreamDatabaseSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            client = new BackblazeClient();
            client.Connect(settings.Credentials!.AccessKeyId, settings.Credentials!.AccessKey);
        }

        public IS3Client CreateClient()
        {
            return new B2S3Client(settings, client!);
        }

        public IS3StreamLock CreateLock(IS3TransactionContext ctx)
        {
            var inMemoryLock = new S3StreamInMemoryLock(ctx.StreamId, storage);
            var fileLock = new S3FileLock(ctx, this);
            return new S3CompositeStreamLock(inMemoryLock, fileLock);
        }
    }
}
