using System;

using Bytewizer.Backblaze.Client;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.B2
{
    internal class B2S3Factory : IS3Factory
    {
        readonly B2StreamDatabaseSettings settings;
        readonly S3InMemoryStreamLockStorage storage;
        readonly BackblazeClient? client; //TODO: create pool of clients

        public B2S3Factory(B2StreamDatabaseSettings settings, S3InMemoryStreamLockStorage storage)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            client = CreateB2Client();
        }

        public IS3Client CreateClient()
        {
            return new B2S3Client(settings, client!);
        }

        public IS3StreamLock CreateLock(Id streamId)
        {
            var inMemoryLock = new S3StreamInMemoryLock(streamId, storage);
            var fileLock = new S3FileLock(streamId, this);
            return new S3CompositeStreamLock(inMemoryLock, fileLock);
        }

        BackblazeClient CreateB2Client()
        {
            var client = new BackblazeClient();
            client.Connect(settings.Credentials!.AccessKeyId, settings.Credentials!.AccessKey);

            return client;
        }
    }
}
