using System;

using B2Net;
using B2Net.Models;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.B2
{
    internal class B2S3Factory : IS3Factory
    {
        readonly B2StreamDatabaseSettings settings;
        readonly S3InMemoryStreamLockStorage storage;
        readonly B2Client? client; //TODO: create pool of clients

        public B2S3Factory(B2StreamDatabaseSettings settings, S3InMemoryStreamLockStorage storage)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            client = CreateB2Client();
            client.Authorize();
        }

        public IS3Client CreateClient()
        {
            return new B2S3Client(settings, client!);
        }

        public IS3StreamLock CreateLock(Id streamId)
        {
            var inMemoryLock = new S3StreamInMemoryLock(streamId, storage);
            var fileLock = new S3FileLock(streamId, CreateClient());
            return new S3CompositeStreamLock(inMemoryLock, fileLock);
        }

        B2Client CreateB2Client()
        {
            var options = new B2Options()
            {
                KeyId = settings.Credentials!.AccessKeyId,
                ApplicationKey = settings.Credentials!.AccessKey,
                BucketId = settings.BucketId,
            };

            return new B2Client(options, authorizeOnInitialize: true);
        }
    }
}
