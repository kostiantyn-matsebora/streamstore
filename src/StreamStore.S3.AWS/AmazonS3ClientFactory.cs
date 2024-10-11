using Amazon.S3;

namespace StreamStore.S3.AWS
{
    internal class AmazonS3ClientFactory : IAmazonS3ClientFactory
    {
        public IAmazonS3 CreateClient()
        {
            return new AmazonS3Client();
        }
    }
}
