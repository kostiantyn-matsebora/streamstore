using System;
using Amazon.S3;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.AWS
{
    internal class AWSS3Factory : S3FactoryBase
    {
        readonly AWSS3DatabaseSettings settings;
        readonly IAmazonS3ClientFactory clientFactory;

        public AWSS3Factory(AWSS3DatabaseSettings settings, IAmazonS3ClientFactory clientFactory)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public override IS3Client CreateClient()
        {
            return new AWSS3Client(clientFactory.CreateClient(), settings);
        }
    }
}
