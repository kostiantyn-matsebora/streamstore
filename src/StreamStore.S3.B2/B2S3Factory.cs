using System;

using Bytewizer.Backblaze.Client;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.B2
{
    internal class B2S3Factory : S3FactoryBase
    {
        readonly B2StreamDatabaseSettings settings;
        readonly IStorageClient? client; //TODO: create pool of clients

        public B2S3Factory(B2StreamDatabaseSettings settings, IStorageClientFactory clientFactory)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            client = clientFactory.Create();
            client!.Connect(settings.Credentials!.AccessKeyId, settings.Credentials!.AccessKey);
        }

        public override IS3Client CreateClient()
        {
            return new B2S3Client(settings, client!);
        }
    }
}
