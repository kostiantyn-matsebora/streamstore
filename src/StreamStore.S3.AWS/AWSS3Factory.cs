using System;
using Amazon.S3;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;

namespace StreamStore.S3.AWS
{
    internal class AWSS3Factory : S3FactoryBase
    {
        readonly AWSS3DatabaseSettings settings;

        public AWSS3Factory(AWSS3DatabaseSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public override IS3Client CreateClient()
        {
            return new AWSS3Client(new AmazonS3Client(), settings);
        }
    }
}
