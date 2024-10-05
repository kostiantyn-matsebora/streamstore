using System;
using Amazon.S3;
using StreamStore.S3.Client;

namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonClientFactory : IS3ClientFactory
    {
        readonly S3StreamDatabaseSettings? settings;

        public AmazonClientFactory(S3StreamDatabaseSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IS3Client CreateClient()
        {
           return new AmazonClient(new AmazonS3Client(), settings!.BucketName!);
        }
    }
}
