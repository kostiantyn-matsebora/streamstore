using System;
using StreamStore.S3.Client;

namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonS3Factory : IS3Factory
    {
        readonly AmazonDatabaseSettings? settings;

        public AmazonS3Factory(AmazonDatabaseSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IS3Client CreateClient()
        {
            return new AmazonClient(settings!);
        }

        public IS3StreamLock CreateLock(Id streamId)
        {
            return new AmazonStreamLock(streamId, settings!.BucketName!, AmazonClient.NewNativeClient(settings));
        }
    }
}
