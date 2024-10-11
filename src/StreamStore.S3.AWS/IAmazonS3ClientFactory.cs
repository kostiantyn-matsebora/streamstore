using Amazon.S3;

namespace StreamStore.S3.AWS
{
    public interface IAmazonS3ClientFactory
    {
        public IAmazonS3 CreateClient();
    }
}
