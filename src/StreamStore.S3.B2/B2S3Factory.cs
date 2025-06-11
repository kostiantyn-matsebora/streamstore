using System;

using Bytewizer.Backblaze.Client;
using StreamStore.S3.Client;

namespace StreamStore.S3.B2
{
    internal class B2S3Factory : S3FactoryBase
    {
        readonly B2StreamStorageSettings settings;
        readonly Lazy<IStorageClient?> client; //TODO: create pool of clients

        public B2S3Factory(B2StreamStorageSettings settings, IStorageClientFactory clientFactory)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            client = new Lazy<IStorageClient?>(() =>
            {
                var c = clientFactory.Create();
                c!.Connect(settings.Credential!.UserName, settings.Credential!.Password);
                return c;
            });
        }

        public override IS3Client CreateClient()
        {
            return new B2S3Client(settings, client.Value!);
        }
    }
}
